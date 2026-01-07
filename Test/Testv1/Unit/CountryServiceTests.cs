using Application.DTOs;
using Application.Services;
using Core.Entities;
using Core.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Test.Unit.Services
{
    public class CountryServiceTests
    {
        [Fact]
        public async Task Create_ShouldFail_WhenCountryExist()
        {
            var repo = new Mock<ICountryRepository>();
            repo.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Country, bool>>>()))
                .ReturnsAsync(true);

            var mockLogger = new Mock<ILogger<CountryService>>();

            var service = new CountryService(repo.Object, mockLogger.Object);
            var dto = new CreateCountryDto("Bolivia");

            var result = await service.CreateAsync(dto);
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Country with the same name already exists.");
        }

        [Fact]
        public async Task Create_ShouldSucceed_WhenValid()
        {
            var repo = new Mock<ICountryRepository>();
            repo.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Country, bool>>>()))
                .ReturnsAsync(false);
            var mockLogger = new Mock<ILogger<CountryService>>();
            var service = new CountryService(repo.Object, mockLogger.Object);
            var dto = new CreateCountryDto("Bolivia");
            var result = await service.CreateAsync(dto);
            result.IsSuccess.Should().BeTrue();
        }
    }
}
