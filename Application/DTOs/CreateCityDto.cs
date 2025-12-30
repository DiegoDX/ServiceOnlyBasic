namespace Application.DTOs
{
    public record CreateCityDto(string Name, long? Population, decimal? Area, string? Description, bool IsCapital, Guid CountryId);
}