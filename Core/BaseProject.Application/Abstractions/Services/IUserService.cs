using BaseProject.Application.DTOs;
using BaseProject.Application.DTOs.Page;
using BaseProject.Application.DTOs.User;
using BaseProject.Application.Paging;
using BaseProject.Domain.Entities;

namespace BaseProject.Application.Abstractions.Services;

public interface IUserService
{
    //User GetAsync(string username);
    Task<RegisteredDto> UserRegister(UserRegisterDTO request);

    Task<IPaginate<User>> GetUsers(PageRequest pageRequest);
}
