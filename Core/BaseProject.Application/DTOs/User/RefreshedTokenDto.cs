using BaseProject.Domain.Entities;
using CorePackages.Security.JWT;

namespace BaseProject.Application.DTOs.User;

public class RefreshedTokenDto
{
    public AccessToken AccessToken { get; set; }
    public RefreshToken RefreshToken { get; set; }
}
