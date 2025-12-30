namespace Application.DTOs
{
    public record CityListItemDto(Guid Id, string Name, long? Population, decimal? Area, string? Description, bool IsCapital, Guid CountryId);
}