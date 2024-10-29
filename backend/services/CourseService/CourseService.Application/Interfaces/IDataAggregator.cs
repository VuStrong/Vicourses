using CourseService.Application.Dtos.Category;
using CourseService.Application.Dtos.Section;

namespace CourseService.Application.Interfaces
{
    public interface IDataAggregator
    {
        Task<List<CategoryWithSubsDto>> GetRootCategoriesWithSubCategoriesAsync();

        Task<List<SectionWithLessonsDto>> GetSectionsWithLessonsAsync(string courseId);
    }
}
