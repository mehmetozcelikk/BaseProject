using BaseProject.Application.Abstractions;
using BaseProject.Persistence.Contexts;
using CorePackages.Persistence.Repositories;
using CorePackages.Security.Entities;

namespace BaseProject.Persistence.Concretes;

public class RefreshTokenManager : EfRepositoryBase<RefreshToken, BaseDbContext>, IRefreshTokenService
{
    public RefreshTokenManager(BaseDbContext context) : base(context)
    {
    }
}
