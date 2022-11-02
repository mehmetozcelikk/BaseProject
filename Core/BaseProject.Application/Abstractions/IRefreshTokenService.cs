using CorePackages.Persistence.Repositories;
using CorePackages.Security.Entities;

namespace BaseProject.Application.Abstractions;

public interface IRefreshTokenService : IAsyncRepository<RefreshToken>, IRepository<RefreshToken>
{
}
