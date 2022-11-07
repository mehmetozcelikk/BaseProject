using BaseProject.Domain.Entities;
using CorePackages.Security.JWT;

namespace BaseProject.Application.Abstractions.Services;

public interface IAuthService
{
    public Task<AccessToken> CreateAccessToken(User user);
    public Task<RefreshToken> CreateRefreshToken(User user, string ipAddress);
    public Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken);
}
