using BaseProject.Domain.Entities;

namespace BaseProject.Application.Repositories.EntityRepositories;

public interface IOperationClaimRepository : IAsyncRepository<OperationClaim>, IRepository<OperationClaim>
{
}
