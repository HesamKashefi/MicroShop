namespace Common.Data
{
    public class PagedResult
    {
        public const int PageSize = 10;
        public required int TotalCount { get; init; }
        public required int TotalPages { get; init; }
        public required int CurrentPage { get; init; }
    }

    public class PagedResult<T> : PagedResult
    {
        public required T Data { get; init; }
    }
}
