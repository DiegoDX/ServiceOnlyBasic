using Application.Common;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Pagination;
using Application.Pagination.Params;
using Core.Constants;
using Core.Entities;
using Core.Exceptions;
using Core.ValueObjects;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryService> _logger;
        private readonly IMemoryCache _cache;
        private const string AllCountriesCacheKey = "all_countries";


        public CountryService(IUnitOfWork uow, ILogger<CountryService> logger, IMemoryCache cache)
        {
            _unitOfWork = uow;
            _logger = logger;
            _cache = cache; 
        }

        public async Task<Result<IEnumerable<CountryListItemDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            if(_cache.TryGetValue(AllCountriesCacheKey, out IEnumerable<CountryListItemDto>? cachedCountries))
            {
                return Result<IEnumerable<CountryListItemDto>>.Success(cachedCountries);
            }

            var countries = await _unitOfWork.Countries.GetAllAsync(cancellationToken);
            var countryDtos = countries.Select(c =>
                new CountryListItemDto(
                    c.Id,
                    c.Name,
                    c.Code?.ToString() ?? string.Empty, // Ensure non-null string for Code
                    c.Population,
                    c.AreaInSquareKm
                )
            );

            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };

            _cache.Set(AllCountriesCacheKey, countryDtos, options);

            return Result<IEnumerable<CountryListItemDto>>.Success(countryDtos);
        }

        public async Task<Guid> CreateAsync(CreateCountryDto dto, CancellationToken cancellationToken)
        {
            var exists = await _unitOfWork.Countries
        .ExistsAsync(c => c.Code == new CountryCode(dto.Code), cancellationToken);

            if (exists)
                throw new DomainException("Country with this code already exists.");

            var country = new Country(dto.Name, new CountryCode(dto.Code), dto.Population, dto.Area);
            await _unitOfWork.Countries.AddAsync(country, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _cache.Remove(AllCountriesCacheKey);
            return country.Id;
        }

        public async Task UpdateAsync(Guid id, CreateCountryDto dto, CancellationToken cancellationToken)
        {
            var existingCountry = await _unitOfWork.Countries.GetByIdAsync(id, cancellationToken);
            if (existingCountry is null)
            {
                _logger.LogWarning("Country {NotFound} with id {CountryId}", Constants.NotFound, id);
                throw new NotFoundException($"Country {Constants.NotFound}.");
            }

            if (await _unitOfWork.Countries.ExistsAsync(x => x.Name == dto.Name && x.Id != id, cancellationToken))
            {
                throw new DomainException("Country with the same name already exists.");
            }

            existingCountry.Name = dto.Name;
            existingCountry.Code = new CountryCode(dto.Code);
            existingCountry.Population = dto.Population;
            existingCountry.AreaInSquareKm = dto.Area;  

            await _unitOfWork.Countries.UpdateAsync(existingCountry, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _cache.Remove(AllCountriesCacheKey);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var existingCountry = await _unitOfWork.Countries.GetByIdAsync(id, cancellationToken);

            if (existingCountry is null)
            {
                throw new NotFoundException($"Country {Constants.NotFound}.");
            }
            
            await _unitOfWork.Countries.DeleteAsync(existingCountry, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _cache.Remove(AllCountriesCacheKey);
        }

        public async Task<PagedResult<CountryListItemDto>> GetPagedAsync(CountryQueryParams query, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Countries
            .GetPagedAsync(query, cancellationToken);

            return new PagedResult<CountryListItemDto>(
                result.Items.Select(c => new CountryListItemDto(
                    c.Id,
                    c.Name,
                    c.Code?.ToString() ?? string.Empty,
                    c.Population,
                    c.AreaInSquareKm
                )).ToList(),
                result.TotalCount,
                query.Page,
                query.PageSize);
        }

        public async Task<CountryListItemDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var country = await _unitOfWork.Countries.GetByIdAsync(id, cancellationToken);
            if (country == null)
            {
                // You may want to throw or handle this case according to your application's error handling strategy.
                // For now, throw to match the non-nullable contract.
                throw new NotFoundException($"Country {Constants.NotFound}.");
            }

            return new CountryListItemDto(
                country.Id,
                country.Name,
                country.Code?.ToString() ?? string.Empty,
                country.Population,
                country.AreaInSquareKm
            );
        }
    }
}
