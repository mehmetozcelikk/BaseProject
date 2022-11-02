using CorePackages.Persistence.Repositories;
using CorePackages.Security.Entities;

namespace BaseProject.Application.Abstractions;

public interface IUserOperationClaimService : IAsyncRepository<UserOperationClaim>, IRepository<UserOperationClaim>
{
}
