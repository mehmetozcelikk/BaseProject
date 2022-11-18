using BaseProject.Application.Repositories.EntityRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Application.Repositories.UnitOfWork;

public interface IUnitOfWork  : IAsyncDisposable ,IDisposable
{

    IEmailAuthenticatorRepository emailAuthenticatorRepository { get; }

    IOperationClaimRepository operationClaimRepository { get; }
    IOtpAuthenticatorRepository otpAuthenticatorRepository { get; }
    IRefreshTokenRepository refreshTokenRepository { get; }

    IUserRepository userRepository { get; }
    IUserOperationClaimRepository userOperationClaimRepository { get; }

    Task<int> SaveAsync();
}
