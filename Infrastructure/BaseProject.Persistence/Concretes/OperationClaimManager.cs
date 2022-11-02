using BaseProject.Application.Abstractions;
using BaseProject.Persistence.Contexts;
using CorePackages.Persistence.Repositories;
using CorePackages.Security.Entities;

namespace BaseProject.Persistence.Concretes;

public class OperationClaimManager : EfRepositoryBase<OperationClaim, BaseDbContext>, IOperationClaimService
{
    public OperationClaimManager(BaseDbContext context) : base(context)
    {
    }
}
