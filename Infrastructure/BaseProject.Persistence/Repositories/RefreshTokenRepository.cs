using BaseProject.Application.Repositories;
using BaseProject.Application.Repositories.EntityRepositories;
using BaseProject.Domain.Entities;
using BaseProject.Persistence.Contexts;

namespace BaseProject.Persistence.Repositories;

public class RefreshTokenRepository : EfRepositoryBase<RefreshToken, BaseDbContext>, IRefreshTokenRepository
{
    public RefreshTokenRepository(BaseDbContext context) : base(context)
    {
    }
}
