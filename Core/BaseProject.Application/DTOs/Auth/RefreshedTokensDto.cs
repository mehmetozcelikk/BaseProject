using BaseProject.Domain.Entities;
using CorePackages.Security.JWT;

namespace BaseProject.Application.DTOs.Auth;

public class RefreshedTokensDto
{
    public AccessToken AccessToken { get; set; }
    public RefreshToken RefreshToken { get; set; }
}
