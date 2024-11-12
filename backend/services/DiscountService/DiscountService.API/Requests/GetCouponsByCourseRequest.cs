using System.ComponentModel.DataAnnotations;

namespace DiscountService.API.Requests
{
    public class GetCouponsByCourseRequest
    {
        /// <summary>
        /// Skip the specified number of items
        /// </summary>
        public int Skip { get; set; } = 0;

        /// <summary>
        /// Limit the number of items returned
        /// </summary>
        [Range(Int32.MinValue, 100, ErrorMessage = "Limit cannot greater than 100")]
        public int Limit { get; set; } = 10;

        /// <summary>
        /// Filter by course id
        /// </summary>
        [Required]
        public string CourseId { get; set; } = null!;

        /// <summary>
        /// Filter by expired or not
        /// </summary>
        public bool? IsExpired { get; set; }
    }
}
