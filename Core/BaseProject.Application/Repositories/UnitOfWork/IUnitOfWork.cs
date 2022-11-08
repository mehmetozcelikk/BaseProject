using BaseProject.Application.Repositories.EntityRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Application.Repositories.UnitOfWork;

public interface IUnitOfWork  : IDisposable
{
    //IUserRepository userRepository { get; }
    //IUserOperationClaimRepository userOperationClaimRepository { get; }
    //IRefreshTokenRepository refreshTokenRepository { get; }
    //IOperationClaimRepository operationClaimRepository { get; }
    //Task<int> SaveAsync();
}
