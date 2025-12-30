using Application.Common;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Pagination;
using Application.Pagination.Params;
using Core.Constants;
using Core.Entities;
using Core.Exceptions;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.Services
{
    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CityService> _logger;

        public CityService(IUnitOfWork uow, ILogger<CityService> logger)
        {
            _unitOfWork = uow;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<CityListItemDto>>> GetAllAsync(Guid countryId, CancellationToken cancellationToken)
        {
            var cities = await _unitOfWork.Cities.GetAllAsync(countryId, cancellationToken);
            var cityDtos = cities.Select(c => new CityListItemDto(
                c.Id,
                c.Name,
                c.Population,
                c.AreaInSquareKm,
                c.Description,
                c.IsCapital,
                c.CountryId
            ));
            return Result<IEnumerable<CityListItemDto>>.Success(cityDtos);
        }

        public async Task<Guid> CreateAsync(CreateCityDto dto, CancellationToken cancellationToken)
        {
            var city = new City
            (
                dto.Name,
                dto.CountryId,
                dto.Population,
                dto.Area,
                dto.Description,
                dto.IsCapital
            );
            await _unitOfWork.Cities.AddAsync(city, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return city.Id;
        }

        public async Task UpdateAsync(UpdateCityDto cityDto, CancellationToken cancellationToken)
        {
            var city = await _unitOfWork.Cities.GetByAsync(c => c.Id == cityDto.Id, cancellationToken);

            if (city.Name != cityDto.Name 
                && await _unitOfWork.Cities.ExistsAsync(cityDto.Name, cityDto.CountryId, cancellationToken))
            {
                throw new NotFoundException("City already exists.");
            }

            city.Name = cityDto.Name;
            city.CountryId = cityDto.CountryId;
            city.Description = cityDto.Description;
            city.Population = cityDto.Population;
            city.CountryId = cityDto.CountryId;
            city.IsCapital = cityDto.IsCapital;
            city.AreaInSquareKm = cityDto.Area;
            await _unitOfWork.Cities.UpdateAsync(city, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var city = await _unitOfWork.Cities.GetByAsync(c => c.Id == id, cancellationToken);

            if (city == null)
            {
                _logger.LogWarning("City {NotFound} with id {CityId}", Constants.NotFound, id);
                throw new NotFoundException($"City {Constants.NotFound}.");
            }

            await _unitOfWork.Cities.DeleteAsync(city, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<PagedResult<CityListItemDto>> GetPagedAsync(Guid countryId, CityQueryParams query, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Cities
           .GetByCountryPagedAsync(countryId, query, cancellationToken);

            return new PagedResult<CityListItemDto>(
                result.Items.Select(c => new CityListItemDto(
                    c.Id,
                    c.Name,
                    c.Population,
                    c.AreaInSquareKm,
                    c.Description,
                    c.IsCapital,
                    c.CountryId
                )).ToList(),
                result.TotalCount,
                query.Page,
                query.PageSize);
        }

        public async Task<City?> GetByIdAsync(Guid Id, CancellationToken cancellationToken)
        {
            var city = await _unitOfWork.Cities.GetByAsync(c => c.Id == Id, cancellationToken);
            return city;
        }
    }
}