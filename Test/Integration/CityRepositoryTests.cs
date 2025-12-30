using Core.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Test.Integration
{
    public class CityRepositoryTests
    {
        [Fact]
        public async Task Repository_ShouldSave_City()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var db = new AppDbContext(options);
            var repo = new CityRepository(db);

            await repo.AddAsync(new City { Name = "Santa Cruz", CountryId = 1 });
            await db.SaveChangesAsync();

            var exist = await repo.ExistsAsync("Santa Cruz", 1);

            exist.Should().BeTrue();
        }
    }
}
