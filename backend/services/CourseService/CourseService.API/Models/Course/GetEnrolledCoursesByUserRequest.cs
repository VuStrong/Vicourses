﻿using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Models.Course
{
    public class GetEnrolledCoursesByUserRequest : PagingRequest
    {
        /// <summary>
        /// ID of user
        /// </summary>
        [Required]
        public string UserId { get; set; } = null!;
    }
}
