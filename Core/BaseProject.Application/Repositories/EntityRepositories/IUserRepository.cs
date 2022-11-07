using BaseProject.Domain.Entities;

namespace BaseProject.Application.Repositories.EntityRepositories;

public interface IUserRepository : IAsyncRepository<User>, IRepository<User>
{
}
