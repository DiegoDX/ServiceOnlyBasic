namespace Application.DTOs
{
    public record UpdateCityDto(Guid Id, string Name, long? Population, decimal? Area, string? Description, bool IsCapital, Guid CountryId);
}