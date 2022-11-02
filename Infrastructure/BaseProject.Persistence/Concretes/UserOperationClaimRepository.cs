using BaseProject.Application.Abstractions;
using BaseProject.Persistence.Contexts;
using CorePackages.Persistence.Repositories;
using CorePackages.Security.Entities;

namespace BaseProject.Persistence.Concretes;

public class UserOperationClaimRepository : EfRepositoryBase<UserOperationClaim, BaseDbContext>, IUserOperationClaimService
{
    public UserOperationClaimRepository(BaseDbContext context) : base(context)
    {
    }
}

