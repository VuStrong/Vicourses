using CourseService.Application.Dtos.Course;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Models.Course
{
    public class GetCoursesRequest
    {
        /// <summary>
        /// Skip the specified number of courses
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// Limit the number of courses returned
        /// </summary>
        [Range(Int32.MinValue, 100, ErrorMessage = "Limit cannot greater than 100")]
        public int Limit { get; set; }

        /// <summary>
        /// Keyword to search courses
        /// </summary>
        [FromQuery(Name = "q")]
        public string? Keyword { get; set; }

        /// <summary>
        /// Filter courses by main category ID
        /// </summary>
        public string? CategoryId { get; set; }

        /// <summary>
        /// Filter courses by sub category ID
        /// </summary>
        public string? SubCategoryId { get; set; }

        public GetCoursesParamsDto ToGetCoursesDto()
        {
            return new GetCoursesParamsDto
            {
                Skip = Skip,
                Limit = Limit,
                SearchKeyword = Keyword,
                CategoryId = CategoryId,
                SubCategoryId = SubCategoryId
            };
        }
    }
}
