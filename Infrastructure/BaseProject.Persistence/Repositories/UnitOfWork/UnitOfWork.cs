using BaseProject.Application.Repositories.EntityRepositories;
using BaseProject.Application.Repositories.UnitOfWork;
using BaseProject.Persistence.Contexts;

namespace BaseProject.Persistence.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        //private readonly BaseDbContext _context;
        //public IUserRepository userRepository { get; private set; }
        //public IUserOperationClaimRepository userOperationClaimRepository { get; private set; }
        //public IRefreshTokenRepository refreshTokenRepository { get; private set; }
        //public IOperationClaimRepository operationClaimRepository { get; private set; }


        //public UnitOfWork(BaseDbContext context)
        //{
        //    _context = context;
        //    userRepository = new UserRepository(_context);
        //    userOperationClaimRepository = new UserOperationClaimRepository(_context);
        //    refreshTokenRepository = new RefreshTokenRepository(_context);
        //    operationClaimRepository = new OperationClaimRepository(_context);
        //}


        //public void Dispose()
        //{
        //    _context.Dispose();
        //}

        //public async Task<int> SaveAsync()
        //{
        //    return await _context.SaveChangesAsync();
        //}
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
