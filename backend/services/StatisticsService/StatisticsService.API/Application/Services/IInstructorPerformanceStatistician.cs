using StatisticsService.API.Application.Dtos;

namespace StatisticsService.API.Application.Services
{
    public interface IInstructorPerformanceStatistician
    {
        Task<InstructorPerformanceDto> GetInstructorPerformanceAsync(string instructorId, DateScope dateScope = DateScope.Month, 
            string? courseId = null, CancellationToken cancellationToken = default);
    }
}
