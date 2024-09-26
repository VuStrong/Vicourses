using CourseService.Domain.Models;

namespace CourseService.Domain.Contracts
{
    public interface ICourseCurriculumManager
    {
        Task<Section?> GetSectionByIdAsync(string id);
        Task CreateSectionAsync(Section section);
        Task UpdateSectionAsync(Section section);
        Task DeleteSectionAsync(string sectionId);

        Task<Lession?> GetLessionByIdAsync(string id);
        Task CreateLessionAsync(Lession lession);
        Task UpdateLessionAsync(Lession lession);
        Task DeleteLessionAsync(string lessionId);

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
