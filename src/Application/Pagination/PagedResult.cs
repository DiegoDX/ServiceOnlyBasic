namespace Application.Pagination
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; }
        public int Page { get; }
        public int PageSize { get; }

        public int TotalPages { get; set; }
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;

        public PagedResult()
        {
        }

        public PagedResult(
            IReadOnlyList<T> items,
            int totalCount,
            int pageNumber,
            int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            Page = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
