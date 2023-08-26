namespace Common.Data
{
    public record Result(DomainStatusCodes Status)
    {
        public string StatusText => Status.ToString();

        #region Success
        public static Result Success() => new(DomainStatusCodes.OK);
        public static Result<TData> Success<TData>(TData data) where TData : class => new(data, DomainStatusCodes.OK);
        public static PagedResult<TData> Success<TData>(TData data, Pager? Pager) where TData : class => new(data, Pager, DomainStatusCodes.OK);
        #endregion


        #region Fail
        public static Result Fail(DomainStatusCodes domainStatusCodes) => new(domainStatusCodes);
        public static Result<TData> Fail<TData>(DomainStatusCodes domainStatusCodes) where TData : class => new(null, domainStatusCodes);
        #endregion

        #region Common Fail Methods
        public static Result NotFound() => new(DomainStatusCodes.NotFound);
        public static Result Forbidden() => new(DomainStatusCodes.Forbidden);
        public static Result<TData> NotFound<TData>() where TData : class => new(null, DomainStatusCodes.NotFound);
        public static Result<TData> Forbidden<TData>() where TData : class => new(null, DomainStatusCodes.Forbidden);
        #endregion
    }

    public record Result<TData>(TData? Data, DomainStatusCodes Status) : Result(Status) where TData : class
    {
    }

    public record PagedResult<TData>(TData? Data, Pager? Pager, DomainStatusCodes Status) : Result<TData>(Data, Status) where TData : class
    {
        public static new PagedResult<TData> Fail(DomainStatusCodes domainStatusCodes) => new(null, null, domainStatusCodes);
    }

    public record Pager(int TotalCount, int CurrentPage, int PageSize = Pager.DefaultPageSize)
    {
        public const int DefaultPageSize = 10;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }

    public enum DomainStatusCodes : short
    {
        OK = 0,

        NotFound = 10,
        UnAuthorized = 11,
        Forbidden = 12,

        BadRequest = 20,
        DuplicateRequest = 21,

        ServerError = 50
    }
}
