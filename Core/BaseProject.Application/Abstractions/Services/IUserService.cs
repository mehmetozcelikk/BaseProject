using BaseProject.Application.DTOs;
using BaseProject.Application.DTOs.User;
using BaseProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BaseProject.Application.Abstractions.Services;

public interface IUserService
{
    //User GetAsync(string username);
    Task<RegisteredDto> UserRegister(UserRegisterDTO request  );

    List<User> GetUsers();
}
