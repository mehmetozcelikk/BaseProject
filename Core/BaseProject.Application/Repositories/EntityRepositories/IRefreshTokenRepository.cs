using BaseProject.Domain.Entities;

namespace BaseProject.Application.Repositories.EntityRepositories;

public interface IRefreshTokenRepository : IAsyncRepository<RefreshToken>, IRepository<RefreshToken>
{

}
