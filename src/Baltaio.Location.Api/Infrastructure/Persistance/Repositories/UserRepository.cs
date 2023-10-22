using Baltaio.Location.Api.Application.Users.Abstractions;
using Baltaio.Location.Api.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Baltaio.Location.Api.Infrastructure.Persistance.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        bool exists = await _context.Users.AnyAsync(u => u.Email == email);
        return exists;
    }

    public async Task<User?> LoginAsync(User userToSearch, CancellationToken cancellationToken = default)
    {
        User? user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == userToSearch.Email && u.Password == userToSearch.Password, cancellationToken);
        return user;
    }
    public async Task SaveAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user);
    }
}
