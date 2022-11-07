using BaseProject.Application.Repositories;
using BaseProject.Domain.Entities;

namespace BaseProject.Application.Abstractions.Services;

public interface IUserOperationClaimService : IAsyncRepository<UserOperationClaim>, IRepository<UserOperationClaim>
{
}
