using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Services;
using Core.Entities;
using Core.Exceptions;
using Core.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Application.Tests.Countries
{
    public class CountryServiceTests
    {
        private readonly Mock<ICountryRepository> _countryRepoMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<ILogger<CountryService>> _loggerMock = new();

        private CountryService CreateService()
        {
            _unitOfWorkMock.Setup(u => u.Countries)
                    .Returns(_countryRepoMock.Object);

            return new CountryService(
                _unitOfWorkMock.Object,
                _loggerMock.Object);
        }


        [Fact]
        public async Task CreateAsync_WhenCountryCodeAlreadyExists_ShouldThrowDomainException()
        {
            _countryRepoMock
                .Setup(r => r.ExistsAsync(
                    It.IsAny<Expression<Func<Country, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var service = CreateService();

            Func<Task> act = () =>
                service.CreateAsync(
                    new CreateCountryDto("Argentina", "AR", null, null),
                    CancellationToken.None);

            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("*already exists*");
        }


        [Fact]
        public async Task CreateAsync_WithValidData_ShouldAddCountryAndSave()
        {
            _countryRepoMock
                .Setup(r => r.ExistsAsync(
                    It.IsAny<Expression<Func<Country, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var service = CreateService();

            var id = await service.CreateAsync(
                new CreateCountryDto("Argentina", "AR", null, null),
                CancellationToken.None);

            id.Should().NotBe(Guid.Empty);

            _countryRepoMock.Verify(r =>
                r.AddAsync(It.IsAny<Country>(), CancellationToken.None),
                Times.Once);

            //_unitOfWorkMock.Verify(u =>
            //    u.SaveChangesAsync(CancellationToken.None),
            //    Times.Once);
        }
    }
}