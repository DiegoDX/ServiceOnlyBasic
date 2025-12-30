using Application.Interfaces.Repositories;
using Application.Pagination.Params;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly AppDbContext _context;

        public CityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(City city, CancellationToken cancellationToken)
        {
           await _context.Cities.AddAsync(city, cancellationToken);
        }

        public Task DeleteAsync(City city, CancellationToken cancellationToken)
        {
            _context.Cities.Remove(city);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(City city, CancellationToken cancellationToken)
        {
            _context.Cities.Update(city);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<City>> GetAllAsync(Guid countryId, CancellationToken cancellationToken)
        {
              return await _context.Cities.AsNoTracking()
                .Include(c => c.Country)
                .Where(c => c.CountryId == countryId)
                .ToListAsync(cancellationToken);
        }

        public async Task<City?> GetByAsync(Expression<Func<City, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _context.Cities.AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public Task<bool> ExistsAsync(string name, Guid countryId, CancellationToken cancellationToken)
        {
            return _context.Cities.AsNoTracking().AnyAsync(c => c.Name == name && c.CountryId == countryId, cancellationToken);
        }

        public async Task<(IEnumerable<City> Items, int TotalCount)> GetByCountryPagedAsync(Guid countryId, CityQueryParams cityQueryParam, CancellationToken cancellationToken = default)
        {
            var query = _context.Cities.AsNoTracking()
                .Include(c => c.Country)
                .Where(c => c.CountryId == countryId);

            var totalCount = await query.CountAsync(cancellationToken);

            var cities = await query
                .OrderBy(c => c.Name)
                .Skip(cityQueryParam.Skip)
                .Take(cityQueryParam.Take)
                .ToListAsync(cancellationToken);

            return (cities, totalCount);
        }
    }
}