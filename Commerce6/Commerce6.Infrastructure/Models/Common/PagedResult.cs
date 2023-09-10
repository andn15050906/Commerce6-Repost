namespace Commerce6.Infrastructure.Models.Common
{
    public class PagedResult<T>
    {
        public int TotalCount { get; init; }
        public int PageIndex { get; init; }
        public int PageSize { get; init; }
        public int PageCount { get => (int)Math.Ceiling((double)TotalCount / PageSize); }

        public List<T> Items { get; init; }
    }
}
