using Microsoft.AspNetCore.Identity;

namespace BaseProject.Domain.Entities.Identity;

public class User : IdentityUser<string>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public byte[] PasswordSalt { get; set; }
    public byte[] PasswordHash { get; set; }
    public bool Status { get; set; }
    public User()
    {
        UserOperationClaims = new HashSet<UserOperationClaim>();
        RefreshTokens = new HashSet<RefreshToken>();
    }
    public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; }
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
}
