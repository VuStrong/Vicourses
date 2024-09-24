using CourseService.Application.Dtos.Course;
using CourseService.Application.Dtos.Lession;
using CourseService.Application.Dtos.Section;
using CourseService.Domain.Contracts;

namespace CourseService.Application.Services
{
    public interface ICourseCurriculumService
    {
        Task<SectionDto> GetSectionByIdAsync(string id);
        Task<LessionDto> GetLessionByIdAsync(string id);

        Task<SectionDto> CreateSectionAsync(CreateSectionDto data, string courseOwnerId);

        Task<LessionDto> CreateLessionAsync(CreateLessionDto data, string courseOwnerId);

        Task<CoursePublicCurriculumDto> GetPublicCurriculumAsync(string courseId);
        Task UpdateCurriculumAsync(string courseId, List<CurriculumItem> items, string courseOwnerId);
    }
}
