using CourseService.Domain.Objects;

namespace CourseService.Domain.Contracts
{
    public interface ICourseCurriculumRepository
    {
        Task<List<SectionWithLessions>> GetCourseCurriculumAsync(string courseId);
        Task UpdateCurriculumAsync(string courseId, List<CurriculumItem> items);
    }

    public class CurriculumItem
    {
        public required string Id { get; set; }
        public required CurriculumItemType Type { get; set; }
    }

    public enum CurriculumItemType
    {
        Section,
        Lession,
    }
}
