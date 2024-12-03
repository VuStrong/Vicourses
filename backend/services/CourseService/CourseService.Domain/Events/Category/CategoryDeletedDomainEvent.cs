namespace CourseService.Domain.Events.Category
{
    public class CategoryDeletedDomainEvent : DomainEvent
    {
        public Models.Category Category { get; set; }

        public CategoryDeletedDomainEvent(Models.Category category)
        {
            Category = category;
        }
    }
}
