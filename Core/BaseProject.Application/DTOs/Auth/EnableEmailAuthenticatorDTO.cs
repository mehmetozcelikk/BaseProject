namespace BaseProject.Application.DTOs.Auth
{
    public class EnableEmailAuthenticatorDTO
    {
        public int UserId { get; set; }
        public string VerifyEmailUrlPrefix { get; set; }
    }
}
