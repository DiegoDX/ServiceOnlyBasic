using Application.Interfaces.Repositories;
using Application.Pagination.Params;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _context;

        public CountryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Country country, CancellationToken cancellationToken)
        {
            await _context.Countries.AddAsync(country,cancellationToken);
        }

        public Task DeleteAsync(Country country, CancellationToken cancellationToken)
        {
            _context.Countries.Remove(country);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Country country, CancellationToken cancellationToken)
        {
            _context.Countries.Update(country);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Country>> GetAllAsync(CancellationToken cancellationToken)
             => await _context.Countries.ToListAsync(cancellationToken);

        public async Task<bool> ExistsAsync(Expression<Func<Country, bool>> predicate, CancellationToken cancellationToken)
            =>  await _context.Countries.AnyAsync(predicate, cancellationToken);

        public async Task<Country?> GetByIdAsync(Guid countryId, CancellationToken cancellationToken)
        {
            return await _context.Countries.FirstOrDefaultAsync(c => c.Id == countryId, cancellationToken);
        }

        public async Task<(IEnumerable<Country> Items, int TotalCount)> GetPagedAsync(CountryQueryParams query, CancellationToken cancellationToken)
        {
            IQueryable<Country> q = _context.Countries.AsQueryable();
            

            // 🔍 Search
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                q = q.Where(x => x.Name.Contains(query.Search));
            }

            // 🔃 Sorting
            q = query.SortBy?.ToLower() switch
            {
                "name" => query.SortDirection == "desc"
                    ? q.OrderByDescending(x => x.Name)
                    : q.OrderBy(x => x.Name),

                _ => q.OrderBy(x => x.Name)
            };

            var totalCount = await q.CountAsync(cancellationToken);

            var items = await q
                .Skip(query.Skip)
                .Take(query.Take)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}