using Application.Pagination.Params;
using Core.Entities;
using System.Linq.Expressions;

namespace Application.Interfaces.Repositories
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetAllAsync(Guid countryId, CancellationToken cancellationToken);

        Task<(IEnumerable<City> Items, int TotalCount)>
        GetByCountryPagedAsync(Guid countryId, CityQueryParams query, CancellationToken cancellationToken);
        Task AddAsync(City city, CancellationToken cancellationToken);
        Task UpdateAsync(City city, CancellationToken cancellationToken);
        Task DeleteAsync(City city, CancellationToken cancellationToken);
        Task<City?> GetByAsync(Expression<Func<City, bool>> predicate, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(string name, Guid countryId, CancellationToken cancellationToken);
    }
}