using CorePackages.Security.JWT;

namespace BaseProject.Application.DTOs.User;

public class UpdatedUserFromAuthDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public AccessToken AccessToken { get; set; }
}
