namespace PaymentService.API.Application.Dtos
{
    public class PagedResult<T>
    {
        public int Skip { get; private set; }
        public int Limit { get; private set; }
        public long Total { get; private set; }
        public bool End { get; private set; }
        public IEnumerable<T> Items { get; private set; }

        public PagedResult(IEnumerable<T> items, int skip, int limit, long total)
        {
            Items = items;
            Skip = skip;
            Limit = limit;
            Total = total;
            End = limit + skip >= total;
        }
    }
}
