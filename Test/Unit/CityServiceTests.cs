using Application.DTOs;
using Application.Services;
using Core.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.Unit.Unit
{
    public class CityServiceTests
    {
        [Fact]
        public async Task Create_ShouldFail_WhenCityExist()
        {
            var repo = new Mock<ICityRepository>();
            repo.Setup(r => r.ExistsAsync("Santa Cruz",1))
                .ReturnsAsync(true);

            var mockLogger = new Mock<ILogger<CityService>>();

            var service = new CityService(repo.Object, mockLogger.Object);
            var dto = new CreateCityDto("Santa Cruz",1);

            var result = await service.CreateAsync(dto);
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("City already exists.");
        }

        [Fact]
        public async Task Create_ShouldSucceed_WhenValid()
        {
            var repo = new Mock<ICityRepository>();
            repo.Setup(r => r.ExistsAsync("Santa Cruz", 1))
                .ReturnsAsync(false);

            var mockLogger = new Mock<ILogger<CityService>>();
            var service = new CityService(repo.Object, mockLogger.Object);
            var dto = new CreateCityDto("Santa Cruz", 1);
            var result = await service.CreateAsync(dto);
            result.IsSuccess.Should().BeTrue();
        }
    }
}
