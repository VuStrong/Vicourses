namespace CourseService.Shared.Paging
{
    public class PagedResult<T>
    {
        public int Skip { get; private set; }
        public int Limit { get; private set; }
        public long Total { get; private set; }
        public List<T> Items { get; private set; }

        public PagedResult(List<T> items, int skip, int limit, long total)
        {
            Items = items;
            Skip = skip;
            Limit = limit;
            Total = total;
        }
    }
}
