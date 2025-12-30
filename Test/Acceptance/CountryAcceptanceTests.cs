using Application.DTOs;
using Application.Services;
using Core.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net;
using WebAPI.Controllers;

namespace Test.Acceptance
{
    public class CountryAcceptanceTests : IDisposable
    {

        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<AppDbContext> _options;

        public CountryAcceptanceTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            // schema creation
            using var context = new AppDbContext(_options);
            context.Database.EnsureCreated();
        }

        private AppDbContext CreateContext()
        => new AppDbContext(_options);

        [Fact]
        public async Task CreateCountry_ShouldSucceed_WhenValid()
        {
            // Arrange
            using var context = CreateContext();
            var repo = new CountryRepository(context);
            var logger = NullLogger<CountryService>.Instance;
            var service = new CountryService(repo, logger);
            var controller = new CountriesController(service);

            var dto = new CreateCountryDto("Bolivia");

            // Act
            var result = await controller.Create(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var saved = await context.Countries.FirstOrDefaultAsync();
            saved.Should().NotBeNull();
            saved!.Name.Should().Be("Bolivia");
        }

        [Fact]
        public async Task GetAllCountries_ShouldReturnCreatedCountry()
        {
            // Arrange
            using var context = CreateContext();
            var repo = new CountryRepository(context);
            var logger = NullLogger<CountryService>.Instance;
            var service = new CountryService(repo, logger);
            var controller = new CountriesController(service);

            // Seed
            context.Countries.Add(new Country { Name = "Chile" });
            context.SaveChanges();

            // Act
            var result = await controller.Getall();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var countries = okResult.Value as IEnumerable<CountryDto>;
            countries.Should().NotBeNull();
            countries.Should().Contain(c => c.Name == "Chile");
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}