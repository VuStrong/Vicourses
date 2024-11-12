namespace DiscountService.API.Models
{
    public class Course
    {
        public string Id { get; private set; }
        public string CreatorId { get; private set; }
        public decimal Price { get; set; }

        public bool IsFree { get => Price == 0; }

        public Course(string id, string creatorId, decimal price)
        {
            Id = id;
            CreatorId = creatorId;
            Price = price;
        }
    }
}
