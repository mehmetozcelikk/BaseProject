using BaseProject.Application.Abstractions.Services;
using BaseProject.Application.DTOs.Auth;
using BaseProject.Application.DTOs.User;
using BaseProject.Application.Repositories.UnitOfWork;
using BaseProject.Domain;
using BaseProject.Domain.Entities;
using CorePackages.CrossCuttingConcerns.Exceptions;
using CorePackages.Mailing;
using CorePackages.Security.EmailAuthenticator;
using CorePackages.Security.JWT;
using CorePackages.Security.OtpAuthenticator;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace BaseProject.Persistence.Concretes;

public class AuthManager : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenHelper _tokenHelper;
    private readonly IOtpAuthenticatorHelper _otpAuthenticatorHelper;
    private readonly TokenOptions _tokenOptions;
    private readonly IEmailAuthenticatorHelper _emailAuthenticatorHelper;
    private readonly IMailService _mailService;

    public AuthManager(IUnitOfWork unitOfWork, ITokenHelper tokenHelper, IOtpAuthenticatorHelper otpAuthenticatorHelper, TokenOptions tokenOptions, IEmailAuthenticatorHelper emailAuthenticatorHelper, IMailService mailService)
    {
        _unitOfWork = unitOfWork;
        _tokenHelper = tokenHelper;
        _otpAuthenticatorHelper = otpAuthenticatorHelper;
        _tokenOptions = tokenOptions;
        _emailAuthenticatorHelper = emailAuthenticatorHelper;
        _mailService = mailService;
    }

    //public async Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken)
    //{
    //    RefreshToken addedRefreshToken = await _unitOfWork.refreshTokenRepository.AddAsync(refreshToken);
    //    return addedRefreshToken;
    //}

    //public async Task<AccessToken> CreateAccessToken(User user)
    //{
    //    IPaginate<UserOperationClaim> userOperationClaims =
    //       await _unitOfWork.userOperationClaimRepository.GetListAsync(u => u.UserId == user.Id,
    //                                                        include: u =>
    //                                                            u.Include(u => u.OperationClaim)
    //       );
    //    IList<OperationClaim> operationClaims =
    //        userOperationClaims.Items.Select(u => new OperationClaim
    //        { Id = u.OperationClaim.Id, Name = u.OperationClaim.Name }).ToList();

    //    AccessToken accessToken = _tokenHelper.CreateToken(user, operationClaims);
    //    return accessToken;
    //}

    //public async Task<RefreshToken> CreateRefreshToken(User user, string ipAddress)
    //{
    //    RefreshToken refreshToken = _tokenHelper.CreateRefreshToken(user, ipAddress);
    //    return await Task.FromResult(refreshToken);
    //}










    public async Task<AccessToken> CreateAccessToken(User user)
    {
        IList<OperationClaim> operationClaims = await _unitOfWork.userOperationClaimRepository
                .Query()
                .AsNoTracking()
                .Where(p => p.UserId == user.Id)
                .Select(p => new OperationClaim
                {
                    Id = p.OperationClaimId,
                    Name = p.OperationClaim.Name
                })
                .ToListAsync();

        AccessToken accessToken = _tokenHelper.CreateToken(user, operationClaims);
        return accessToken;
    }

    public async Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken)
    {
        RefreshToken addedRefreshToken = await _unitOfWork.refreshTokenRepository.AddAsync(refreshToken);
        return addedRefreshToken;
    }

    public async Task DeleteOldRefreshTokens(int userId)
    {
        IList<RefreshToken> refreshTokens = (await _unitOfWork.refreshTokenRepository.GetListAsync(r =>
                                                 r.UserId == userId &&
                                                 r.Revoked == null && r.Expires >= DateTime.UtcNow &&
                                                 r.Created.AddDays(_tokenOptions.RefreshTokenTTL) <=
                                                 DateTime.UtcNow)
                                            ).Items;
        foreach (RefreshToken refreshToken in refreshTokens) await _unitOfWork.refreshTokenRepository.DeleteAsync(refreshToken);
    }

    public async Task<RefreshToken?> GetRefreshTokenByToken(string token)
    {
        RefreshToken? refreshToken = await _unitOfWork.refreshTokenRepository.GetAsync(r => r.Token == token);
        return refreshToken;
    }

    public async Task RevokeRefreshToken(RefreshToken refreshToken, string ipAddress, string? reason = null,
                                         string? replacedByToken = null)
    {
        refreshToken.Revoked = DateTime.UtcNow;
        refreshToken.RevokedByIp = ipAddress;
        refreshToken.ReasonRevoked = reason;
        refreshToken.ReplacedByToken = replacedByToken;
        await _unitOfWork.refreshTokenRepository.UpdateAsync(refreshToken);
    }

    public async Task<RefreshToken> RotateRefreshToken(User user, RefreshToken refreshToken, string ipAddress)
    {
        RefreshToken newRefreshToken = _tokenHelper.CreateRefreshToken(user, ipAddress);
        await RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }

    public async Task RevokeDescendantRefreshTokens(RefreshToken refreshToken, string ipAddress,
                                                    string reason)
    {
        RefreshToken childToken = await _unitOfWork.refreshTokenRepository.GetAsync(r => r.Token == refreshToken.ReplacedByToken);

        if (childToken != null && childToken.Revoked != null && childToken.Expires <= DateTime.UtcNow)
            await RevokeRefreshToken(childToken, ipAddress, reason);
        else await RevokeDescendantRefreshTokens(childToken, ipAddress, reason);
    }

    public Task<RefreshToken> CreateRefreshToken(User user, string ipAddress)
    {
        RefreshToken refreshToken = _tokenHelper.CreateRefreshToken(user, ipAddress);
        return Task.FromResult(refreshToken);
    }

    public async Task<EmailAuthenticator> CreateEmailAuthenticator(User user)
    {
        EmailAuthenticator emailAuthenticator = new()
        {
            UserId = user.Id,
            ActivationKey = await _emailAuthenticatorHelper.CreateEmailActivationKey(),
            IsVerified = false
        };
        return emailAuthenticator;
    }

    public async Task<OtpAuthenticator> CreateOtpAuthenticator(User user)
    {
        OtpAuthenticator otpAuthenticator = new()
        {
            UserId = user.Id,
            SecretKey = await _otpAuthenticatorHelper.GenerateSecretKey(),
            IsVerified = false
        };
        return otpAuthenticator;
    }

    public async Task<string> ConvertSecretKeyToString(byte[] secretKey)
    {
        string result = await _otpAuthenticatorHelper.ConvertSecretKeyToString(secretKey);
        return result;
    }

    public async Task SendAuthenticatorCode(User user)
    {
        if (user.AuthenticatorType is AuthenticatorType.Email) await sendAuthenticatorCodeWithEmail(user);
    }

    public async Task VerifyAuthenticatorCode(User user, string authenticatorCode)
    {
        if (user.AuthenticatorType is AuthenticatorType.Email)
            await verifyAuthenticatorCodeWithEmail(user, authenticatorCode);
        else if (user.AuthenticatorType is AuthenticatorType.Otp)
            await verifyAuthenticatorCodeWithOtp(user, authenticatorCode);
    }

    private async Task sendAuthenticatorCodeWithEmail(User user)
    {
        EmailAuthenticator emailAuthenticator = await _unitOfWork.emailAuthenticatorRepository.GetAsync(e => e.UserId == user.Id);

        if (!emailAuthenticator.IsVerified) throw new BusinessException("Email Authenticator must be is verified.");

        string authenticatorCode = await _emailAuthenticatorHelper.CreateEmailActivationCode();
        emailAuthenticator.ActivationKey = authenticatorCode;
        await _unitOfWork.emailAuthenticatorRepository.UpdateAsync(emailAuthenticator);

        var toEmailList = new List<MailboxAddress>
            {
                new($"{user.FirstName} {user.LastName}",user.Email)
            };

        _mailService.SendMail(new Mail
        {
            ToList = toEmailList,
            Subject = "Authenticator Code - RentACar",
            TextBody = $"Enter your authenticator code: {authenticatorCode}"
        });
    }

    private async Task verifyAuthenticatorCodeWithEmail(User user, string authenticatorCode)
    {
        EmailAuthenticator emailAuthenticator = await _unitOfWork.emailAuthenticatorRepository.GetAsync(e => e.UserId == user.Id);

        if (emailAuthenticator.ActivationKey != authenticatorCode)
            throw new BusinessException("Authenticator code is invalid.");

        emailAuthenticator.ActivationKey = null;
        await _unitOfWork.emailAuthenticatorRepository.UpdateAsync(emailAuthenticator);
    }

    private async Task verifyAuthenticatorCodeWithOtp(User user, string authenticatorCode)
    {
        OtpAuthenticator otpAuthenticator = await _unitOfWork.otpAuthenticatorRepository.GetAsync(e => e.UserId == user.Id);

        bool result = await _otpAuthenticatorHelper.VerifyCode(otpAuthenticator.SecretKey, authenticatorCode);

        if (!result)
            throw new BusinessException("Authenticator code is invalid.");
    }























    public async Task EnableEmailAuthenticator()
    {
        //User user = await _userService.GetById(request.UserId);
        //await _authBusinessRules.UserShouldBeExists(user);
        //await _authBusinessRules.UserShouldNotBeHaveAuthenticator(user);

        //user.AuthenticatorType = AuthenticatorType.Email;
        //await _userService.Update(user);

        //EmailAuthenticator emailAuthenticator = await _authService.CreateEmailAuthenticator(user);
        //EmailAuthenticator addedEmailAuthenticator =
        //    await _emailAuthenticatorRepository.AddAsync(emailAuthenticator);

        //var toEmailList = new List<MailboxAddress>
        //    {
        //        new($"{user.FirstName} {user.LastName}",user.Email)
        //    };

        //_mailService.SendMail(new Mail
        //{
        //    ToList = toEmailList,
        //    Subject = "Verify Your Email - RentACar",
        //    TextBody =
        //        $"Click on the link to verify your email: {request.VerifyEmailUrlPrefix}?ActivationKey={HttpUtility.UrlEncode(addedEmailAuthenticator.ActivationKey)}"
        //});

        //return Unit.Value; //todo: return dto?
    }


    //public async Task<UserForLoginDto> Login(UserForLoginDto request)
    //{
    //    User? user = await userService.GetByEmail(request.Email);
    //    await _authBusinessRules.UserShouldBeExists(user);
    //    await _authBusinessRules.UserPasswordShouldBeMatch(user.Id, request.Password);

    //    LoggedDto loggedDto = new();

    //    if (user.AuthenticatorType is not AuthenticatorType.None)
    //    {
    //        if (request.UserForLoginDto.AuthenticatorCode is null)
    //        {
    //            await _authService.SendAuthenticatorCode(user);
    //            loggedDto.RequiredAuthenticatorType = user.AuthenticatorType;
    //            return loggedDto;
    //        }

    //        await _authService.VerifyAuthenticatorCode(user, request.AuthenticatorCode);
    //    }

    //    AccessToken createdAccessToken = await _authService.CreateAccessToken(user);

    //    RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(user, request.IPAddress);
    //    RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);
    //    await _authService.DeleteOldRefreshTokens(user.Id);

    //    loggedDto.AccessToken = createdAccessToken;
    //    loggedDto.RefreshToken = addedRefreshToken;
    //    return loggedDto;
    //}




}