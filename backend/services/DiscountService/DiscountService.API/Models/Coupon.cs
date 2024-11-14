using System.Text.Json.Serialization;

namespace DiscountService.API.Models
{
    public class Coupon
    {
        [JsonInclude]
        public string Id { get; private set; }

        [JsonInclude]
        public string Code { get; private set; }

        [JsonInclude]
        public string CreatorId { get; private set; }

        [JsonInclude]
        public string CourseId { get; private set; }
        public Course Course { get; private set; } = null!;

        [JsonInclude]
        public DateTime CreatedAt { get; private set; }

        [JsonInclude]
        public DateTime ExpiryDate { get; private set; }

        [JsonInclude]
        public int Count { get; private set; }

        [JsonInclude]
        public int Remain { get; private set; }

        [JsonInclude]
        public int Discount { get; private set; }
        public bool IsActive { get; set; }

        public bool IsExpired { get => ExpiryDate <= DateTime.Today; }

#pragma warning disable CS8618
        public Coupon() { }

        private Coupon(string id, string code, string creatorId, string courseId)
        {
            Id = id;
            Code = code;
            CreatorId = creatorId;
            CourseId = courseId;
        }

        public static Coupon Create(string code, string creatorId, string courseId, int days, int count, int discount)
        {
            var id = Guid.NewGuid().ToString();

            return new Coupon(id, code, creatorId, courseId)
            {
                CreatedAt = DateTime.Now,
                ExpiryDate = DateTime.Today.AddDays(days),
                Count = count,
                Remain = count,
                Discount = discount,
                IsActive = true,
            };
        }
    }
}
