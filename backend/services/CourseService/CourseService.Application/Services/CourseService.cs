using AutoMapper;
using CourseService.Application.Dtos.Course;
using CourseService.Application.Exceptions;
using CourseService.Domain.Constracts;
using CourseService.Domain.Models;
using Microsoft.Extensions.Logging;

namespace CourseService.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CourseService> _logger;
        public CourseService(
            ICourseRepository courseRepository,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository,
            IEnrollmentRepository enrollmentRepository,
            IMapper mapper,
            ILogger<CourseService> logger)
        {
            _courseRepository = courseRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _enrollmentRepository = enrollmentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<CourseDto>> GetCoursesAsync()
        {
            var courses = await _courseRepository.GetAllAsync();

            return _mapper.Map<List<CourseDto>>(courses);
        }

        public async Task<CourseDetailDto> GetCourseDetailByIdAsync(string courseId)
        {
            var course = await _courseRepository.FindOneAsync(courseId);

            if (course == null)
            {
                throw new CourseNotFoundException(courseId);
            }

            return _mapper.Map<CourseDetailDto>(course);
        }

        public async Task<CourseDto> CreateCourseAsync(CreateCourseDto data)
        {
            var category = await _categoryRepository.FindOneAsync(data.CategoryId);
            if (category == null)
            {
                throw new CategoryNotFoundException(data.CategoryId);
            }

            var user = await _userRepository.FindOneAsync(data.UserId);
            if (user == null)
            {
                throw new UserNotFoundException(data.UserId);
            }

            var course = Course.Create(
                data.Title,
                data.Description,
                new CategoryInCourse(category.Id, category.Name, category.Slug),
                new UserInCourse(user.Id, user.Name, user.ThumbnailUrl));

            await _courseRepository.CreateAsync(course);

            _logger.LogInformation("A course was created with Id = {Id}", course.Id);

            return _mapper.Map<CourseDto>(course);
        }

        public async Task<CourseDto> UpdateCourseAsync(string courseId, UpdateCourseDto data, string ownerId)
        {
            var course = await _courseRepository.FindOneAsync(courseId);

            if (course == null)
            {
                throw new CourseNotFoundException(courseId);
            }

            if (course.User.Id != ownerId)
            {
                throw new ForbiddenException($"Forbidden resourse");
            }

            CategoryInCourse? categoryToUpdate = null;
            if (data.CategoryId != null && data.CategoryId != course.Category.Id)
            {
                var category = await _categoryRepository.FindOneAsync(data.CategoryId) ?? 
                    throw new CategoryNotFoundException(data.CategoryId);

                categoryToUpdate = new CategoryInCourse(category.Id, category.Name, category.Slug);
            }

            ImageFile? thumbnail = data.Thumbnail != null ? new ImageFile()
            {
                FileId = data.Thumbnail.FileId,
                Url = data.Thumbnail.Url,
            } : null;
            VideoFile? previewVideo = data.PreviewVideo != null ? new VideoFile()
            {
                FileId = data.PreviewVideo.FileId,
                Url = data.PreviewVideo.Url,
                FileName = data.PreviewVideo.FileName,
            } : null;

            course.UpdateInfo(data.Title, data.Description, data.Tags, data.Requirements, data.TargetStudents,
                data.LearnedContents, data.Price, data.Language, thumbnail, previewVideo, categoryToUpdate);

            await _courseRepository.UpdateAsync(course);

            return _mapper.Map<CourseDto>(course);
        }

        public async Task DeleteCourseAsync(string courseId, string ownerId)
        {
            var course = await _courseRepository.FindOneAsync(courseId);
            
            if (course == null)
            {
                throw new CourseNotFoundException(courseId);
            }

            if (course.User.Id != ownerId)
            {
                throw new ForbiddenException($"Forbidden resourse");
            }

            if (await _enrollmentRepository.ExistsAsync(e => e.CourseId == courseId))
            {
                throw new ForbiddenException(
                    $"The course {courseId} cannot be deleted because it already has students enrolled"
                );
            }

            await _courseRepository.DeleteOneAsync(courseId);

            _logger.LogInformation($"Course {courseId} deleted");
        }

        public async Task Enroll(string courseId, string userId)
        {
            if (!(await _courseRepository.ExistsAsync(courseId)))
            {
                throw new CourseNotFoundException(courseId);
            }

            var enrollment = Enrollment.Create(courseId, userId);

            await _enrollmentRepository.CreateAsync(enrollment);

            await _courseRepository.IncreaseStudentCount(courseId, 1);

            _logger.LogInformation($"[Course Service] User {userId} enrolled course {courseId}");
        }

        public async Task<bool> CheckEnrollment(string courseId, string userId)
        {
            return await _enrollmentRepository.ExistsAsync(e => e.CourseId == courseId && e.UserId == userId);
        }
    }
}
