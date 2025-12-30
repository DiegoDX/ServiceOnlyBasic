namespace Application.Pagination.Params
{
    public class CountryQueryParams : PaginationParams
    {
        public string? Search { get; init; }
        public string? SortBy { get; init; }
        public string? SortDirection { get; init; }
    }
}
