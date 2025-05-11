using aton.domain.Interfaces;
using aton.infrastructure.Data;

namespace aton.infrastructure.Repositories;

internal class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private bool _disposed = false;

    private IUserRepository? _userRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IUserRepository UserRepository =>
       _userRepository ??= new UserRepository(_context);

    public async Task BeginTransaction() => await _context.Database.BeginTransactionAsync();
    public async Task CommitTransaction() => await _context.Database.CommitTransactionAsync();
    public async Task RollbackTransaction() => await _context.Database.RollbackTransactionAsync();
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
            _context.Dispose();
        _disposed = true;
    }
    public bool IsContextDisposed()
    {
        return _context == null || _context.ChangeTracker == null;
    }
}
