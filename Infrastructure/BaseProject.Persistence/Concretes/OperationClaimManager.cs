using BaseProject.Application.Abstractions.Services;
using BaseProject.Application.Repositories;
using BaseProject.Domain.Entities;
using BaseProject.Persistence.Contexts;

namespace BaseProject.Persistence.Concretes;

public class OperationClaimManager : EfRepositoryBase<OperationClaim, BaseDbContext>, IOperationClaimService
{
    public OperationClaimManager(BaseDbContext context) : base(context)
    {
    }
}
