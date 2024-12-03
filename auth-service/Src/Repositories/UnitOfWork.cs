using auth_service.Data;
using auth_service.Repositories;
using auth_service.Repositories.Interfaces;

namespace auth_service.Services;

public class UnitOfWork : IUnitOfWork
{
        private IRolesRepository _rolesRepository = null!;
        private IUsersRepository _usersRepository = null!;
        private ITokensRepository _tokensRepository = null!;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        private readonly DataContext _context;
        private bool _disposed = false;
        

        public IRolesRepository RolesRepository
        {
            get
            {
                _rolesRepository ??= new RolesRepository(_context);
                return _rolesRepository;
            }
        }

        public IUsersRepository UsersRepository
        {
            get
            {
                _usersRepository ??= new UsersRepository(_context);
                return _usersRepository;
            }
        }

        public ITokensRepository TokensRepository
        {
            get
            {
                _tokensRepository ??= new TokensRepository(_context);
                return _tokensRepository;
            }
        }

    
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing) _context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }