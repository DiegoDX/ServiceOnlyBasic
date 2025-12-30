namespace Application.DTOs
{
    public record CountryListItemDto(Guid Id, string Name, string Code, long? Population, decimal? Area);
}