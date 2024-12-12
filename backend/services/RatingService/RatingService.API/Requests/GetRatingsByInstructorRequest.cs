using System.ComponentModel.DataAnnotations;

namespace RatingService.API.Requests
{
    public class GetRatingsByInstructorRequest
    {
        /// <summary>
        /// Skip the specified number of items
        /// </summary>
        public int Skip { get; set; } = 0;

        /// <summary>
        /// Limit the number of items returned
        /// </summary>
        [Range(Int32.MinValue, 100, ErrorMessage = "Limit cannot greater than 100")]
        public int Limit { get; set; } = 15;

        /// <summary>
        /// Filter by course id
        /// </summary>
        public string? CourseId { get; set; }

        /// <summary>
        /// Filter by rating star
        /// </summary>
        [Range(0, 5)]
        public int? Star { get; set; }

        /// <summary>
        /// Filter by responded or not
        /// </summary>
        public bool? Responded { get; set; }
    }
}
