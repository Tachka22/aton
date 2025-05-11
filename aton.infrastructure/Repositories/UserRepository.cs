using aton.domain.Entities;
using aton.domain.Interfaces;
using aton.infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace aton.infrastructure.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<User> Create(User user)
    {
        await _context.AddAsync(user);
        return user;
    }
    public async Task<IEnumerable<User>> GetAllActive()
    {
        return await _context.Users.Where(w => w.RevokedOn == null).OrderBy(o => o.CreatedOn).ToListAsync();
    }

    public async Task<IEnumerable<User>> GetAllByAge(DateTime minDateTime)
    {
       return await _context.Users.Where(w => w.Birthday >= minDateTime).ToListAsync();
    }

    public async Task<User> GetByLogin(string login)
    {
        return await _context.Users.FirstOrDefaultAsync(f => f.Login == login);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }
}
