using Application.DTOs;
using Application.Interfaces.Services;

namespace WebAPI.GraphQL.Mutations
{
    //CityMutation = Escritura(WRITE)
    public class CityMutation
    {
        public async Task<CreateCityDto> CreateCityAsync(CreateCityDto input, [Service] ICityService cityService, CancellationToken cancellationToken)
        {
            var cityId = await cityService.CreateAsync(input, cancellationToken);
            return new CreateCityDto(
            input.Name,
            input.Population,
            input.Area,
            input.Description,
            input.IsCapital,
            input.CountryId
            );
        }
    }
}
