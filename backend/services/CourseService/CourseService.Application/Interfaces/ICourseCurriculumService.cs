using CourseService.Application.Dtos.Course;
using CourseService.Application.Dtos.Lesson;
using CourseService.Application.Dtos.Section;
using CourseService.Domain.Contracts;

namespace CourseService.Application.Interfaces
{
    public interface ICourseCurriculumService
    {
        Task<SectionDto> GetSectionByIdAsync(string id);
        Task<SectionDto> CreateSectionAsync(CreateSectionDto data);
        Task<SectionDto> UpdateSectionAsync(string sectionId, UpdateSectionDto data, string ownerId);
        Task DeleteSectionAsync(string sectionId, string ownerId);

        Task<LessonDto> GetLessonByIdAsync(string id);
        Task<LessonDto> CreateLessonAsync(CreateLessonDto data);
        Task<LessonDto> UpdateLessonAsync(string lessonId, UpdateLessonDto data, string ownerId);
        Task DeleteLessonAsync(string lessonId, string ownerId);

        Task<LessonDto> AddQuizToLessonAsync(string lessonId, CreateLessonQuizDto data);
        Task<LessonDto> UpdateQuizInLessonAsync(string lessonId, UpdateLessonQuizDto data);
        Task<LessonDto> RemoveQuizFromLessonAsync(string lessonId, int quizNumber, string ownerId);
        Task<LessonDto> MoveQuizInLessonAsync(string lessonId, int quizNumber, int to, string ownerId);

        Task<CoursePublicCurriculumDto> GetPublicCurriculumAsync(string courseId);
        Task<CourseInstructorCurriculumDto> GetInstructorCurriculumAsync(string courseId);
        Task UpdateCurriculumAsync(string courseId, List<CurriculumItem> items, string courseOwnerId);
    }
}
