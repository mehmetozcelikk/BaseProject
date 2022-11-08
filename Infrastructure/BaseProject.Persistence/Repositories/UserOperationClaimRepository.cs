using BaseProject.Application.Repositories;
using BaseProject.Application.Repositories.EntityRepositories;
using BaseProject.Domain.Entities;
using BaseProject.Persistence.Contexts;

namespace BaseProject.Persistence.Repositories;

public class UserOperationClaimRepository : EfRepositoryBase<UserOperationClaim, BaseDbContext>, IUserOperationClaimRepository
{
    public UserOperationClaimRepository(BaseDbContext context) : base(context)
    {
    }
}
