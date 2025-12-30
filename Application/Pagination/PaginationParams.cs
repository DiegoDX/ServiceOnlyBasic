namespace Application.Pagination
{
    public class PaginationParams
    {
        //private const int MaxPageSize = 50;

        //public int PageNumber { get; init; } = 1;

        //private int _pageSize = 10;
        //public int PageSize
        //{
        //    get => _pageSize;
        //    init => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        //}

        private const int MaxPageSize = 50;

        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;

        public int Skip => (Page - 1) * PageSize;

        public int Take => PageSize > MaxPageSize ? MaxPageSize : PageSize;
    }
}
