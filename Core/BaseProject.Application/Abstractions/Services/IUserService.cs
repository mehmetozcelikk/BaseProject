using BaseProject.Domain.Entities;

namespace BaseProject.Application.Abstractions.Services;

public interface IUserService
{
    //User GetAsync(string username);
    List<User> GetUsers();
}
