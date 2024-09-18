using CourseService.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Models.Course
{
    public class GetCoursesByInstructorRequest : PagingRequest
    {
        /// <summary>
        /// ID of instructor
        /// </summary>
        [Required]
        public string InstructorId { get; set; } = null!;

        /// <summary>
        /// Keyword to search courses
        /// </summary>
        [FromQuery(Name = "q")]
        public string? Keyword { get; set; }

        /// <summary>
        /// Filter courses by status (if authenticated userId is not the same as InstructorId, then this param will set to Published)
        /// </summary>
        [EnumDataType(typeof(CourseStatus))]
        public CourseStatus? Status { get; set; }
    }
}
