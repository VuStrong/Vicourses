using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SearchService.API.Application.Dtos
{
    public class SearchCoursesParams
    {
        /// <summary>
        /// Keyword to search for courses
        /// </summary>
        [FromQuery(Name = "q")]
        public string? Keyword { get; set; }

        /// <summary>
        /// Skip the specified number of courses
        /// </summary>
        public int Skip { get; set; } = 0;

        /// <summary>
        /// Limit the number of courses returned
        /// </summary>
        [Range(0, 100, ErrorMessage = "Limit cannot less than 0 or greater than 100")]
        public int Limit { get; set; } = 15;

        /// <summary>
        /// Sort courses
        /// </summary>
        [EnumDataType(typeof(CourseSort))]
        public CourseSort Sort { get; set; } = CourseSort.Relevance;

        /// <summary>
        /// Filter courses by minimum average rating
        /// </summary>
        [Range(0, 5)]
        public decimal? Rating { get; set; }

        /// <summary>
        /// Filter courses by level
        /// </summary>
        [EnumDataType(typeof(CourseLevel))]
        public CourseLevel? Level { get; set; }

        /// <summary>
        /// Filter free courses or not
        /// </summary>
        public bool? Free { get; set; }
    }
}
