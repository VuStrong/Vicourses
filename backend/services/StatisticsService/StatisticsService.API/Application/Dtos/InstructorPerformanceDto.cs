namespace StatisticsService.API.Application.Dtos
{
    public class InstructorPerformanceDto
    {
        public DateScope Scope { get; set; } = DateScope.Month;
        public int TotalEnrollmentCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<MetricsDto> Metrics { get; set; } = null!;
    }

    public class MetricsDto
    {
        public string Label { get; set; } = string.Empty;
        public int EnrollmentCount { get; set; }
        public decimal Revenue { get; set; }
    }
}
