namespace StatisticsService.API.Models
{
    public class InstructorMetric
    {
        public string Id { get; private set; }
        public string InstructorId { get; private set; }
        public string CourseId { get; private set; }
        public DateOnly Date { get; private set; }
        public int EnrollmentCount { get; private set; }
        public decimal Revenue { get; private set; }
        public int RefundCount { get; private set; }

#pragma warning disable CS8618
        private InstructorMetric() { }

        public InstructorMetric(string instructorId, string courseId, DateOnly? date = null)
        {
            Id = Guid.NewGuid().ToString();
            InstructorId = instructorId;
            CourseId = courseId;
            Date = date != null ? date.Value : DateOnly.FromDateTime(DateTime.Today);
        }

        public void IncreaseEnrollmentCount()
        {
            EnrollmentCount++;
        }

        public void IncreaseRevenue(decimal revenue)
        {
            if (revenue > 0)
            {
                Revenue += revenue;
            }
        }

        public void IncreaseRefundCount()
        {
            RefundCount++;
        }
    }
}
