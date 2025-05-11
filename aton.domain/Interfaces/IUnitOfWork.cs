using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aton.domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository UserRepository { get; }
    Task SaveChangesAsync();
    Task BeginTransaction();
    Task CommitTransaction();
    Task RollbackTransaction();
    bool IsContextDisposed();
}

