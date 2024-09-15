﻿namespace CourseService.Application.Dtos.Course
{
    public class CourseDetailDto : CourseDto
    {
        public string? Description { get; set; }
        public string[] Tags { get; set; } = [];
        public string[] Requirements { get; set; } = [];
        public string[] TargetStudents { get; set; } = [];
    }
}