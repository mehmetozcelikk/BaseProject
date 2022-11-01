using CorePackages.Security.Entities;

namespace BaseProject.Application.Abstractions
{
    public interface IUserService
    {
        List<User> GetUsers();
    }
}
