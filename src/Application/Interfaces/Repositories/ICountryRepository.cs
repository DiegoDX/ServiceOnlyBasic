using Application.Pagination.Params;
using Core.Entities;
using System.Linq.Expressions;

namespace Application.Interfaces.Repositories
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetAllAsync(CancellationToken cancellationToken);
        Task<(IEnumerable<Country> Items, int TotalCount)> GetPagedAsync(CountryQueryParams query, CancellationToken cancellationToken);
        Task<Country?> GetByIdAsync(Guid countryId, CancellationToken cancellationToken);
        Task AddAsync(Country country, CancellationToken cancellationToken);
        Task UpdateAsync(Country country, CancellationToken cancellationToken   );
        Task DeleteAsync(Country country, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Expression<Func<Country,bool>> predicate, CancellationToken cancellationToken);
    }
}
