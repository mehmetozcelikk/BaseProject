using CorePackages.Persistence.Repositories;
using CorePackages.Security.Entities;

namespace BaseProject.Application.Abstractions;

public interface IOperationClaimService : IAsyncRepository<OperationClaim>, IRepository<OperationClaim>
{
}
