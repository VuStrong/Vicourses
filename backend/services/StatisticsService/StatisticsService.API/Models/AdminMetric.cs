namespace StatisticsService.API.Models
{
    public class AdminMetric
    {
        public DateOnly Date { get; private set; }
        public decimal Revenue { get; set; }

        public AdminMetric(DateOnly date)
        {
            Date = date;
        }
    }
}
