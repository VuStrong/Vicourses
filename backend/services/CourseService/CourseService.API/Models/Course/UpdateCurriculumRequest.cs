using CourseService.Domain.Contracts;
using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Models.Course
{
    public class UpdateCurriculumRequest
    {
        [Required]
        public List<UpdateCurriculumRequestItem> Items { get; set; } = [];

        public List<CurriculumItem> ToListOfCurriculumItem()
        {
            var items = new List<CurriculumItem>();

            Items.ForEach(item =>
            {
                items.Add(new CurriculumItem
                {
                    Id = item.Id,
                    Type = item.Type,
                });
            });

            return items;
        }
    }

    public class UpdateCurriculumRequestItem
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        [EnumDataType(typeof(CurriculumItemType))]
        public CurriculumItemType Type { get; set; }
    }
}
