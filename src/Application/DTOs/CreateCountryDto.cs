namespace Application.DTOs
{
    public record CreateCountryDto(string Name, string Code, long? Population, decimal? Area);
}