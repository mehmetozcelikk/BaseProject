using BaseProject.Domain.Entities;

namespace BaseProject.Application.Repositories.EntityRepositories;

public interface IUserOperationClaimRepository : IAsyncRepository<UserOperationClaim>, IRepository<UserOperationClaim>
{
}
