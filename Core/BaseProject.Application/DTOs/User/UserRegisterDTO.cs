namespace BaseProject.Application.DTOs.User;

public class UserRegisterDTO : RefreshedTokenDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IpAdress { get; set; }

}
