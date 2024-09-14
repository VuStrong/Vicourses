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
        private readonly IMapper _mapper;
        private readonly ILogger<CourseService> _logger;
        public CourseService(
            ICourseRepository courseRepository,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ILogger<CourseService> logger)
        {
            _courseRepository = courseRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
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
            var course = await _courseRepository.FindOneAsync(c => c.Id == courseId);

            if (course == null)
            {
                throw new CourseNotFoundException(courseId);
            }

            return _mapper.Map<CourseDetailDto>(course);
        }

        public async Task<CourseDto> CreateCourseAsync(CreateCourseDto data)
        {
            var category = await _categoryRepository.FindOneAsync(c => c.Id == data.CategoryId);
            if (category == null)
            {
                throw new CategoryNotFoundException(data.CategoryId);
            }

            var user = await _userRepository.FindOneAsync(u => u.Id == data.UserId);
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
    }
}
