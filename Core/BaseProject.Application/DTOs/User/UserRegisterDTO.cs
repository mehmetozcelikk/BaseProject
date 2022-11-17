namespace BaseProject.Application.DTOs.User;

public class UserRegisterDTO : RefreshedTokenDto
{
    public UserForRegisterDto userForRegisterDto { get; set; }
    public string IpAdress { get; set; }

}
