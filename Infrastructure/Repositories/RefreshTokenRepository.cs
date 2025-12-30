using Application.Interfaces.Repositories;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        }

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            _context.RefreshTokens.Remove(new RefreshToken { Id = id });
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<RefreshToken>> GetAllWithIncludesAsync(CancellationToken cancellationToken)
        {
            return await _context.RefreshTokens
                .ToListAsync(cancellationToken);
        }

        public async Task<RefreshToken?> GetAsync(string token, CancellationToken cancellationToken)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token, cancellationToken);
        }

        public Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            _context.RefreshTokens.Update(refreshToken);
            return Task.CompletedTask;
        }
    }
}
