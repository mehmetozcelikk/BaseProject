using BaseProject.Application.DTOs;
using BaseProject.Application.DTOs.Page;
using BaseProject.Application.DTOs.User;
using BaseProject.Application.Paging;
using BaseProject.Domain.Entities;

namespace BaseProject.Application.Abstractions.Services;

public interface IUserService
{
    //User GetAsync(string username);
    public Task<User?> GetByEmail(string email);
    public Task<User> GetById(int id);
    public Task<User> Update(User user);

    //Task<RegisteredDto> UserRegister(UserRegisterDTO request);

    Task<IPaginate<User>> GetUsers(PageRequest pageRequest);
}
