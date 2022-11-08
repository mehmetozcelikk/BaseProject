using BaseProject.Application.Repositories;
using BaseProject.Application.Repositories.EntityRepositories;
using BaseProject.Domain.Entities;
using BaseProject.Persistence.Contexts;

namespace BaseProject.Persistence.Repositories;

public class OperationClaimRepository : EfRepositoryBase<OperationClaim, BaseDbContext>, IOperationClaimRepository
{
    public OperationClaimRepository(BaseDbContext context) : base(context)
    {
    }
}
