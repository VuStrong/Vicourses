namespace StatisticsService.API.Application.Dtos
{
    public class AdminDashboardDataDto
    {
        public int TotalStudent { get; set; }
        public int TotalInstructor { get; set; }
        public int TotalPublishedCourse { get; set; }
        public decimal TotalMonthRevenue { get; set; }
        public List<AdminMetricsDto> Metrics { get; set; } = [];
    }

    public class AdminMetricsDto
    {
        public string Label { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
    }
}
