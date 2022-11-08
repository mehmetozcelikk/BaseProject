using AutoMapper;
using BaseProject.Application.DTOs.User;
using BaseProject.Domain.Entities;

namespace BaseProject.Application.Mapping.UserProfiles;

public class UserProfiles : Profile
{
    public UserProfiles()
    {
        CreateMap<User, UserRegisterDTO>().ReverseMap();

    }

}
