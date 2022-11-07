using BaseProject.Application.Repositories;
using BaseProject.Domain.Entities;

namespace BaseProject.Application.Abstractions.Services;

public interface IOperationClaimService : IAsyncRepository<OperationClaim>, IRepository<OperationClaim>
{
}
