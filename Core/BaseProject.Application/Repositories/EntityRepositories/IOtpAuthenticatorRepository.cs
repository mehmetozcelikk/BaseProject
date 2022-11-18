using BaseProject.Domain.Entities;

namespace BaseProject.Application.Repositories.EntityRepositories;

public interface IOtpAuthenticatorRepository : IAsyncRepository<OtpAuthenticator>, IRepository<OtpAuthenticator>
{
}
