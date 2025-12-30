using Application.DTOs;
using Application.Interfaces.Services;
using Application.Pagination;
using Application.Pagination.Params;
using Application.Services;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService service)
        {
            _countryService = service;
        }

        // --------------------------------------------------------------------
        // GET: api/countries?page=1&pageSize=10&search=arg&sortBy=name&sortDir=asc
        // --------------------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<PagedResult<CountryListItemDto>>> GetPaged(
            [FromQuery] CountryQueryParams query,
            CancellationToken cancellationToken)
        {
            var result = await _countryService.GetPagedAsync(query, cancellationToken);
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
            var country = await _countryService.GetByIdAsync(id, cancellationToken);
            return Ok(country);
        }

        // -----------------------------
        // POST: api/countries
        // -----------------------------
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateCountryDto dto,
            CancellationToken cancellationToken)
        {
            var id = await _countryService.CreateAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = id },
                null);
        }

        // -----------------------------
        // PUT: api/countries/{id}
        // -----------------------------
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] CreateCountryDto dto,
            CancellationToken cancellationToken)
        {
            await _countryService.UpdateAsync(id, dto, cancellationToken);
            return NoContent();
        }

        // -----------------------------
        // DELETE: api/countries/{id}
        // -----------------------------
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            await _countryService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}