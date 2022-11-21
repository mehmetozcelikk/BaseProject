namespace BaseProject.Application.DTOs.Auth
{
    public class VerifyOtpAuthenticatorDTO
    {
        public int UserId { get; set; }
        public string ActivationCode { get; set; }
    }
}
