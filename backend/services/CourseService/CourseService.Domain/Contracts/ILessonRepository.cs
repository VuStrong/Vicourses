﻿using CourseService.Domain.Models;

namespace CourseService.Domain.Contracts
{
    public interface ILessonRepository : IRepository<Lesson>
    {
        Task<List<Lesson>> FindBySectionIdAsync(string sectionId);
        Task<List<Lesson>> FindByCourseIdAsync(string courseId);
        Task<long> CountBySectionIdAsync(string sectionId);
        Task<long> CountByCourseIdAsync(string courseId);
    }
}
