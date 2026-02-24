using Application.DTOs;
using Application.Interfaces.Services;

namespace WebAPI.GraphQL.Queries
{
    //🟢 CityQuery = Lectura (READ)
    public class CityQuery
    {
        public async Task<Application.Common.Result<IEnumerable<CityListItemDto>>> GetCitiesByIdAsync(Guid countryId, [Service] ICityService cityService, CancellationToken cancellationToken)
        {
            return await cityService.GetAllAsync(countryId, cancellationToken);
        }
    }
}
