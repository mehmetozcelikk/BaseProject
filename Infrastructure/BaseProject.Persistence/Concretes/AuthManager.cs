using AutoMapper;
using BaseProject.Application.Abstractions.Services;
using BaseProject.Application.Constants;
using BaseProject.Application.DTOs.Auth;
using BaseProject.Application.DTOs.User;
using BaseProject.Application.Repositories.UnitOfWork;
using BaseProject.Domain;
using BaseProject.Domain.Entities;
using CorePackages.CrossCuttingConcerns.Exceptions;
using CorePackages.Mailing;
using CorePackages.Security.EmailAuthenticator;
using CorePackages.Security.Hashing;
using CorePackages.Security.JWT;
using CorePackages.Security.OtpAuthenticator;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Web;

namespace BaseProject.Persistence.Concretes;

public class AuthManager : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenHelper _tokenHelper;
    private readonly IOtpAuthenticatorHelper _otpAuthenticatorHelper;
    //private readonly TokenOptions _tokenOptions;
    private readonly IEmailAuthenticatorHelper _emailAuthenticatorHelper;
    private readonly IMailService _mailService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public AuthManager(IUnitOfWork unitOfWork, ITokenHelper tokenHelper, IOtpAuthenticatorHelper otpAuthenticatorHelper,/* TokenOptions tokenOptions,*/ IEmailAuthenticatorHelper emailAuthenticatorHelper, IMailService mailService, IUserService userService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _tokenHelper = tokenHelper;
        _otpAuthenticatorHelper = otpAuthenticatorHelper;
        //_tokenOptions = tokenOptions;
        _emailAuthenticatorHelper = emailAuthenticatorHelper;
        _mailService = mailService;
        _userService = userService;
        _mapper = mapper;
    }



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
                                                 r.Revoked == null && r.Expires >= DateTime.UtcNow)
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








    //
    public async Task<EnableEmailAuthenticatorDTO> EnableEmailAuthenticator(EnableEmailAuthenticatorDTO request)
    {
        User user = await _userService.GetById(request.UserId);
        if (user == null) throw new BusinessException(AuthMessages.UserDontExists);

        if (user.AuthenticatorType != AuthenticatorType.None)
            throw new BusinessException(AuthMessages.UserHaveAlreadyAAuthenticator);

        user.AuthenticatorType = AuthenticatorType.Email;
        await _userService.Update(user);

        EmailAuthenticator emailAuthenticator = await CreateEmailAuthenticator(user);
        EmailAuthenticator addedEmailAuthenticator =
            await _unitOfWork.emailAuthenticatorRepository.AddAsync(emailAuthenticator);

        var toEmailList = new List<MailboxAddress>
            {
                new($"{user.FirstName} {user.LastName}",user.Email)
            };

        _mailService.SendMail(new Mail
        {
            ToList = toEmailList,
            Subject = "Verify Your Email - RentACar",
            TextBody =
                $"Click on the link to verify your email: {request.VerifyEmailUrlPrefix}?ActivationKey={HttpUtility.UrlEncode(addedEmailAuthenticator.ActivationKey)}"
        });

        return request; //todo: return dto?
    }


    public async Task<EnabledOtpAuthenticatorDto> EnableOtpAuthenticator(EnableOtpAuthenticatorDTO request
                                                          )
    {
        User user = await _userService.GetById(request.UserId);
        if (user == null) throw new BusinessException(AuthMessages.UserDontExists);
        if (user.AuthenticatorType != AuthenticatorType.None)
            throw new BusinessException(AuthMessages.UserHaveAlreadyAAuthenticator);

        await _unitOfWork.otpAuthenticatorRepository.GetAsync(o => o.UserId == request.UserId);
        var isExistsOtpAuthenticator =
            await _unitOfWork.otpAuthenticatorRepository.GetAsync(o => o.UserId == request.UserId);

        if (isExistsOtpAuthenticator is not null && isExistsOtpAuthenticator.IsVerified)
            throw new BusinessException(AuthMessages.AlreadyVerifiedOtpAuthenticatorIsExists); if (isExistsOtpAuthenticator is not null)
            await _unitOfWork.otpAuthenticatorRepository.DeleteAsync(isExistsOtpAuthenticator);

        OtpAuthenticator newOtpAuthenticator = await CreateOtpAuthenticator(user);

        OtpAuthenticator addedOtpAuthenticator =
            await _unitOfWork.otpAuthenticatorRepository.AddAsync(newOtpAuthenticator);   
            
            
            EnabledOtpAuthenticatorDto enabledOtpAuthenticatorDto = new()
        {
            SecretKey = await ConvertSecretKeyToString(addedOtpAuthenticator.SecretKey)
        };
        return enabledOtpAuthenticatorDto;
        

    }

    // Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEwMiIsImVtYWlsIjoic3RyaW5nYWRhc2RzYWRzYWQiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic3RyaW5nIHN0cmluZyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6ImFzZHNhZGFzIiwibmJmIjoxNjY5MTA3MDcwLCJleHAiOjE2NjkxODAyNzAsImlzcyI6InJlbnRhY2FyQHJlbnRhY2FyLmNvbSIsImF1ZCI6InJlbnRhY2FyQHJlbnRhY2FyLmNvbSJ9.4KTJMjLr1RCsqnRwOmZXfdpTK0Z-BKriy3FGlPY1QE-ezJXjfE-PvOBBSKfVUQfQP6FpzH4WbRttLzpol-9tMg

    public async Task<LoggedDto> Login(UserLoginDTO request)
    {
        User? user = await _userService.GetByEmail(request.userForLoginDto.Email);
        if (user == null) throw new BusinessException(AuthMessages.UserDontExists);


        User? user2 = await _unitOfWork.userRepository.GetAsync(u => u.Id == user.Id);
        if (!HashingHelper.VerifyPasswordHash(request.userForLoginDto.Password, user.PasswordHash, user.PasswordSalt))
            throw new BusinessException(AuthMessages.PasswordDontMatch);

        LoggedDto loggedDto = new();

        if (user.AuthenticatorType is not AuthenticatorType.None)
        {
            if (request.userForLoginDto.AuthenticatorCode is null)
            {
                await SendAuthenticatorCode(user);
                loggedDto.RequiredAuthenticatorType = user.AuthenticatorType;
                return loggedDto;
            }

            await VerifyAuthenticatorCode(user, request.userForLoginDto.AuthenticatorCode);
        }

        AccessToken createdAccessToken = await CreateAccessToken(user);

        RefreshToken createdRefreshToken = await CreateRefreshToken(user, request.IPAddress);
        RefreshToken addedRefreshToken = await AddRefreshToken(createdRefreshToken);
        await DeleteOldRefreshTokens(user.Id);

        loggedDto.AccessToken = createdAccessToken;
        loggedDto.RefreshToken = addedRefreshToken;
        return loggedDto;
    }





    public async Task<RefreshedTokensDto> RefreshToken(RefreshTokenDTO request
                                                  )
    {
        RefreshToken? refreshToken = await GetRefreshTokenByToken(request.RefleshToken);
        if (refreshToken == null) throw new BusinessException(AuthMessages.RefreshDontExists);

        if (refreshToken.Revoked != null)
            await RevokeDescendantRefreshTokens(refreshToken, request.IPAddress,
                                                             $"Attempted reuse of revoked ancestor token: {refreshToken.Token}");
        if (refreshToken.Revoked != null && DateTime.UtcNow >= refreshToken.Expires)
            throw new BusinessException(AuthMessages.InvalidRefreshToken);
        User user = await _userService.GetById(refreshToken.UserId);

        RefreshToken newRefreshToken = await RotateRefreshToken(user, refreshToken, request.IPAddress);
        RefreshToken addedRefreshToken = await AddRefreshToken(newRefreshToken);

        await DeleteOldRefreshTokens(refreshToken.UserId);

        AccessToken createdAccessToken = await CreateAccessToken(user);

        RefreshedTokensDto refreshedTokensDto = new()
        { AccessToken = createdAccessToken, RefreshToken = addedRefreshToken };
        return refreshedTokensDto;
    }






    public async Task<RegisteredDto> Register(UserRegisterDTO request)
    {
        byte[] passwordHash, passwordSalt;
        HashingHelper.CreatePasswordHash(request.userForRegisterDto.Password, out passwordHash, out passwordSalt);

        User? user = await _unitOfWork.userRepository.GetAsync(u => u.Email == request.userForRegisterDto.Email);
        if (user != null) throw new BusinessException("Mail already exists");


        var count = _unitOfWork.userRepository.GetList();
        User newUser = new()
        {
            Email = request.userForRegisterDto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            FirstName = request.userForRegisterDto.FirstName,
            LastName = request.userForRegisterDto.LastName,
            Status = true
        };
        User createdUser = _unitOfWork.userRepository.AddAsync(newUser).Result;

        AccessToken createdAccessToken = await CreateAccessToken(createdUser);
        RefreshToken createdRefreshToken = await CreateRefreshToken(createdUser, request.IpAdress);
        RefreshToken addedRefreshToken = await AddRefreshToken(createdRefreshToken);

        RegisteredDto registeredDto = new()
        {
            RefreshToken = addedRefreshToken,
            AccessToken = createdAccessToken,
        };
        return registeredDto;

    }




    public async Task<RevokeTokenDTO> RevokeToken(RevokeTokenDTO request)
    {
        RefreshToken? refreshToken = await GetRefreshTokenByToken(request.Token);
        if (refreshToken == null) throw new BusinessException(AuthMessages.RefreshDontExists);

        if (refreshToken.Revoked != null && DateTime.UtcNow >= refreshToken.Expires)
            throw new BusinessException(AuthMessages.InvalidRefreshToken);
        await RevokeRefreshToken(refreshToken, request.IPAddress, "Revoked without replacement");

        //RevokedTokenDto revokedTokenDto = _mapper.Map<RevokedTokenDto>(refreshToken);
        return request;
    }





    public async Task<VerifyEmailAuthenticatorDTO> VerifyEmailAuthenticator(VerifyEmailAuthenticatorDTO request)
    {
        EmailAuthenticator? emailAuthenticator =
            await _unitOfWork.emailAuthenticatorRepository.GetAsync(
                e => e.ActivationKey == request.ActivationKey);
        if (emailAuthenticator is null) throw new BusinessException(AuthMessages.EmailAuthenticatorDontExists);
        if (emailAuthenticator.ActivationKey is null) throw new BusinessException(AuthMessages.EmailActivationKeyDontExists);

        emailAuthenticator.ActivationKey = null;
        emailAuthenticator.IsVerified = true;
        await _unitOfWork.emailAuthenticatorRepository.UpdateAsync(emailAuthenticator);

        return request;
    }




    public async Task<VerifyOtpAuthenticatorDTO> VerifyOtpAuthenticator(VerifyOtpAuthenticatorDTO request)
    {
        OtpAuthenticator? otpAuthenticator =
            await _unitOfWork.otpAuthenticatorRepository.GetAsync(e => e.UserId == request.UserId);
        if (otpAuthenticator is null) throw new BusinessException(AuthMessages.OtpAuthenticatorDontExists);

        User user = await _userService.GetById(request.UserId);

        otpAuthenticator.IsVerified = true;
        user.AuthenticatorType = AuthenticatorType.Otp;

        if (otpAuthenticator is not null && otpAuthenticator.IsVerified)
            throw new BusinessException(AuthMessages.AlreadyVerifiedOtpAuthenticatorIsExists);

        await _unitOfWork.otpAuthenticatorRepository.UpdateAsync(otpAuthenticator);
        await _userService.Update(user);

        return request;
    }











}