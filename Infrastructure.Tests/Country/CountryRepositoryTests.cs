using Core.ValueObjects;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Country
{
    public class CountryRepositoryTests
    {
        private AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            var context = new AppDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            return context;
        }

        [Fact]
        public async Task AddAsync_ShouldPersistCountry()
        {
            using var context = CreateDbContext();
            var repo = new CountryRepository(context);

            var country = new Core.Entities.Country("Argentina", new CountryCode("AR"), null, null);

            await repo.AddAsync(country, CancellationToken.None);
            await context.SaveChangesAsync();

            context.Countries.Should().HaveCount(1);
        }
    }
}
