using CourseService.Application.Dtos.Course;
using CourseService.Application.Dtos.Lession;
using CourseService.Application.Dtos.Section;
using CourseService.Domain.Contracts;

namespace CourseService.Application.Services
{
    public interface ICourseCurriculumService
    {
        Task<SectionDto> GetSectionByIdAsync(string id);
        Task<SectionDto> CreateSectionAsync(CreateSectionDto data);
        Task<SectionDto> UpdateSectionAsync(string sectionId, UpdateSectionDto data, string ownerId);
        Task DeleteSectionAsync(string sectionId, string ownerId);

        Task<LessionDto> GetLessionByIdAsync(string id);
        Task<LessionDto> CreateLessionAsync(CreateLessionDto data);
        Task<LessionDto> UpdateLessionAsync(string lessionId, UpdateLessionDto data, string ownerId);
        Task DeleteLessionAsync(string lessionId, string ownerId);

        Task<CoursePublicCurriculumDto> GetPublicCurriculumAsync(string courseId);
        Task<CourseInstructorCurriculumDto> GetInstructorCurriculumAsync(string courseId);
        Task UpdateCurriculumAsync(string courseId, List<CurriculumItem> items, string courseOwnerId);
    }
}
