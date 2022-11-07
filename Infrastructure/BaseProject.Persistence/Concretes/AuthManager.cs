using BaseProject.Application.Abstractions.Services;
using BaseProject.Application.Paging;
using BaseProject.Domain.Entities;
using CorePackages.Security.JWT;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Persistence.Concretes;

public class AuthManager : IAuthService
{
    private readonly IUserOperationClaimService _userOperationClaimRepository;
    private readonly ITokenHelper _tokenHelper;
    private readonly IRefreshTokenService _refreshTokenRepository;

    public AuthManager(IUserOperationClaimService userOperationClaimRepository, ITokenHelper tokenHelper, IRefreshTokenService refreshTokenRepository)
    {
        _userOperationClaimRepository = userOperationClaimRepository;
        _tokenHelper = tokenHelper;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken)
    {
        RefreshToken addedRefreshToken = await _refreshTokenRepository.AddAsync(refreshToken);
        return addedRefreshToken;
    }

    public async Task<AccessToken> CreateAccessToken(User user)
    {
        IPaginate<UserOperationClaim> userOperationClaims =
           await _userOperationClaimRepository.GetListAsync(u => u.UserId == user.Id,
                                                            include: u =>
                                                                u.Include(u => u.OperationClaim)
           );
        IList<OperationClaim> operationClaims =
            userOperationClaims.Items.Select(u => new OperationClaim
            { Id = u.OperationClaim.Id, Name = u.OperationClaim.Name }).ToList();

        AccessToken accessToken = _tokenHelper.CreateToken(user, operationClaims);
        return accessToken;
    }

    public async Task<RefreshToken> CreateRefreshToken(User user, string ipAddress)
    {
        RefreshToken refreshToken = _tokenHelper.CreateRefreshToken(user, ipAddress);
        return await Task.FromResult(refreshToken);
    }
}