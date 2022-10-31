using BaseProject.Domain.Entities;

namespace BaseProject.Application.Abstractions
{
    public interface IUserService
    {
        List<User> GetUsers();
    }
}
