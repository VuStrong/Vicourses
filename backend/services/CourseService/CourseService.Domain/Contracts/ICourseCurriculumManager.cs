using CourseService.Domain.Models;

namespace CourseService.Domain.Contracts
{
    public interface ICourseCurriculumManager
    {
        Task<Section?> GetSectionByIdAsync(string id);
        Task CreateSectionAsync(Section section);
        
        Task<Lession?> GetLessionByIdAsync(string id);
        Task CreateLessionAsync(Lession lession);

        Task UpdateCurriculumAsync(List<CurriculumItem> items);
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
