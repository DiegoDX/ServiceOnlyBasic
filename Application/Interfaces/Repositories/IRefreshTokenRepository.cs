using Core.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
        Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<RefreshToken>> GetAllWithIncludesAsync(CancellationToken cancellationToken);
        Task<RefreshToken?> GetAsync(string token, CancellationToken cancellationToken);
    }
}
