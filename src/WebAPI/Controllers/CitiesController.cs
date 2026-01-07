using Application.DTOs;
using Application.Interfaces.Services;
using Application.Pagination;
using Application.Pagination.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CitiesController(ICityService service)
        {
            _cityService = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<CityListItemDto>>> GetPaged([FromQuery]CityQueryParams cityQueryParams, CancellationToken cancellationToken)
        {
            var result = await _cityService.GetPagedAsync(cityQueryParams.CountryId, cityQueryParams, cancellationToken);
            return Ok(result);
        }

        // -----------------------------
        // GET: api/countries/{id}
        // -----------------------------
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CountryListItemDto>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var city = await _cityService.GetByIdAsync(id, cancellationToken);
            return Ok(city);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCityDto dto,
            CancellationToken cancellationToken)
        {
            var id = await _cityService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(
                nameof(GetById),
                new { id },
                null);

        }

        [HttpPut]
        public async Task<IActionResult> Edit(UpdateCityDto dto,
            CancellationToken cancellationToken)
        {
          
            await _cityService.UpdateAsync(dto, cancellationToken);
            return NoContent();
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id,
            CancellationToken cancellationToken)
        {
            await _cityService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}