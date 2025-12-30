using Application.Common;
using Application.DTOs;
using Application.Pagination;
using Application.Pagination.Params;
using Core.Entities;
using System.Linq.Expressions;

namespace Application.Interfaces.Services
{
    public interface ICityService
    {
        Task<Result<IEnumerable<CityListItemDto>>> GetAllAsync(Guid countryId, CancellationToken cancellationToken);
        Task<Guid> CreateAsync(CreateCityDto createCityDto, CancellationToken cancellationToken);
        Task UpdateAsync(UpdateCityDto updateCitydto, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<PagedResult<CityListItemDto>> GetPagedAsync(Guid countryId, CityQueryParams query, CancellationToken cancellationToken);
        Task<City?> GetByIdAsync(Guid Id, CancellationToken cancellationToken);
    }
}
