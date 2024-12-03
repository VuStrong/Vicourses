using AutoMapper;
using CourseService.Application.Dtos.Course;
using CourseService.Application.Exceptions;
using CourseService.Application.Interfaces;
using CourseService.Domain.Contracts;
using CourseService.Domain.Enums;
using CourseService.Shared.Paging;

namespace CourseService.Application.Services
{
    public class EnrollService : IEnrollService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IMapper _mapper;

        public EnrollService(
            ICourseRepository courseRepository,
            IEnrollmentRepository enrollmentRepository,
            IMapper mapper
        )
        {
            _courseRepository = courseRepository;
            _enrollmentRepository = enrollmentRepository;
            _mapper = mapper;
        }

        public async Task EnrollAsync(string courseId, string userId, bool throwIfCourseIsPaid = false)
        {
            if (await CheckEnrollmentAsync(courseId, userId))
            {
                throw new AppException("You are already enrolled", 422);
            }

            var course = await _courseRepository.FindOneAsync(courseId) ?? throw new CourseNotFoundException(courseId);

            if (throwIfCourseIsPaid && course.IsPaid)
            {
                throw new ForbiddenException("This course is paid, you cannot enroll");
            }

            course.EnrollStudent(userId);

            await _courseRepository.UpdateAsync(course);
        }

        public async Task UnenrollAsync(string courseId, string userId)
        {
            var course = await _courseRepository.FindOneAsync(courseId) ?? throw new CourseNotFoundException(courseId);

            course.UnenrollStudent(userId);

            await _courseRepository.UpdateAsync(course);
        }

        public async Task<bool> CheckEnrollmentAsync(string courseId, string userId)
        {
            return await _enrollmentRepository.ExistsAsync(e => e.CourseId == courseId && e.UserId == userId);
        }

        public async Task<PagedResult<CourseDto>> GetUserEnrolledCoursesAsync(string userId, int skip, int limit, 
            bool onlyPublishedCourses = false)
        {
            skip = skip < 0 ? 0 : skip;
            limit = limit <= 0 ? 10 : limit;

            var result = await _enrollmentRepository.FindByUserIdAsync(userId, skip, limit);

            var courseIds = result.Items.Select(i => i.CourseId);
            var courses = await _courseRepository.FindByIdsAsync(
                courseIds,
                onlyPublishedCourses ? CourseStatus.Published : null
            );

            return new PagedResult<CourseDto>(_mapper.Map<List<CourseDto>>(courses), skip, limit, result.Total);
        }
    }
}