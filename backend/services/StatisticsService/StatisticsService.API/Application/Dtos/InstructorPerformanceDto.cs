namespace StatisticsService.API.Application.Dtos
{
    public class InstructorPerformanceDto
    {
        public DateScope Scope { get; set; } = DateScope.Month;
        public int TotalEnrollmentCount { get; set; }
        public int TotalRefundCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<InstructorMetricsDto> Metrics { get; set; } = null!;
    }

    public class InstructorMetricsDto
    {
        public string Label { get; set; } = string.Empty;
        public int EnrollmentCount { get; set; }
        public int RefundCount { get; set; }
        public decimal Revenue { get; set; }
    }
}
