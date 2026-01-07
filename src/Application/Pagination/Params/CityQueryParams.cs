namespace Application.Pagination.Params
{
    public class CityQueryParams : PaginationParams
    {
        public Guid CountryId { get; init; }
        public string? Search { get; init; }
    }
}
