using BaseProject.Application.DTOs.Auth;
using BaseProject.Application.DTOs.User;
using BaseProject.Domain.Entities;
using CorePackages.Security.JWT;

namespace BaseProject.Application.Abstractions.Services;

public interface IAuthService
{



    public Task<AccessToken> CreateAccessToken(User user);
    public Task<RefreshToken> CreateRefreshToken(User user, string ipAddress);
    public Task<RefreshToken?> GetRefreshTokenByToken(string token);
    public Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken);
    public Task DeleteOldRefreshTokens(int userId);
    public Task RevokeDescendantRefreshTokens(RefreshToken refreshToken, string ipAddress, string reason);

    public Task RevokeRefreshToken(RefreshToken token, string ipAddress, string? reason = null,
                                   string? replacedByToken = null);

    public Task<RefreshToken> RotateRefreshToken(User user, RefreshToken refreshToken, string ipAddress);
    public Task<EmailAuthenticator> CreateEmailAuthenticator(User user);
    public Task<OtpAuthenticator> CreateOtpAuthenticator(User user);
    public Task<string> ConvertSecretKeyToString(byte[] secretKey);
    public Task SendAuthenticatorCode(User user);
    public Task VerifyAuthenticatorCode(User user, string authenticatorCode);



    public Task<EnabledOtpAuthenticatorDto> EnableOtpAuthenticator(EnableOtpAuthenticatorDTO request);


    public Task<EnableEmailAuthenticatorDTO> EnableEmailAuthenticator(EnableEmailAuthenticatorDTO request);
    public Task<RefreshedTokensDto> RefreshToken(RefreshTokenDTO request);
    public Task<LoggedDto> Login(UserLoginDTO request);

    public Task<RegisteredDto> Register(UserRegisterDTO request);


    public Task<RevokeTokenDTO> RevokeToken(RevokeTokenDTO request);

    public Task<VerifyEmailAuthenticatorDTO> VerifyEmailAuthenticator(VerifyEmailAuthenticatorDTO request);

    public Task<VerifyOtpAuthenticatorDTO> VerifyOtpAuthenticator(VerifyOtpAuthenticatorDTO request);





}
