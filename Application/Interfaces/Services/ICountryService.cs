using Application.Common;
using Application.DTOs;
using Application.Pagination;
using Application.Pagination.Params;

namespace Application.Interfaces.Services
{
    public interface ICountryService
    {
        Task<Result<IEnumerable<CountryListItemDto>>> GetAllAsync(CancellationToken cancellationToken);
        Task<PagedResult<CountryListItemDto>> GetPagedAsync(CountryQueryParams query, CancellationToken cancellationToken);
        Task<Guid> CreateAsync(CreateCountryDto dto, CancellationToken cancellationToken);
        Task UpdateAsync(Guid id, CreateCountryDto countryDto, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<CountryListItemDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
