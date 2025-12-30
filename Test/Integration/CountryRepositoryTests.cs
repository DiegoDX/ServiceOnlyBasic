using Core.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Test.Integration
{
    public class CountryRepositoryTests
    {
        [Fact]
        public async Task Repository_ShouldSave_Country()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var db = new AppDbContext(options);
            var repo = new CountryRepository(db);

            await repo.AddAsync(new Country { Name = "Chile" });
            await db.SaveChangesAsync();

            var exist = await repo.ExistsAsync(c => c.Name == "Chile");

            exist.Should().BeTrue();
        }
    }
}
