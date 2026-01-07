using Application.Interfaces.Repositories;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _ctx;
        public UserRepository(AppDbContext ctx) => _ctx = ctx;

        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            _ctx.Users.Add(user);
            await _ctx.SaveChangesAsync(cancellationToken);
        }

        public Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail, CancellationToken cancellationToken = default)
        {
            return _ctx.Users
                .FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail, cancellationToken);
        }

        public Task<bool> ExistsAsync(string username, string email, CancellationToken cancellationToken)
        {
            return _ctx.Users.AnyAsync(u => u.Username == username || u.Email == email, cancellationToken);
        }

        public Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default)
        {
            return _ctx.Users
             .FirstOrDefaultAsync(u => u.Id == Id, cancellationToken);
        }
    }
}
