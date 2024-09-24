using AutoMapper;
using CourseService.Application.Dtos.Course;
using CourseService.Application.Dtos.Lession;
using CourseService.Application.Dtos.Section;
using CourseService.Application.Exceptions;
using CourseService.Domain.Contracts;
using CourseService.Domain.Models;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CourseService.Application.Services
{
    public class CourseCurriculumService : ICourseCurriculumService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseCurriculumManager _courseCurriculumManager;
        private readonly IMapper _mapper;
        private readonly ILogger<CourseCurriculumService> _logger;

        public CourseCurriculumService(
            ICourseRepository courseRepository,
            ICourseCurriculumManager courseCurriculumManager,
            IMapper mapper,
            ILogger<CourseCurriculumService> logger)
        {
            _courseRepository = courseRepository;
            _courseCurriculumManager = courseCurriculumManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SectionDto> GetSectionByIdAsync(string id)
        {
            var section = await _courseCurriculumManager.GetSectionByIdAsync(id);

            if (section == null)
            {
                throw new SectionNotFoundException(id);
            }

            return _mapper.Map<SectionDto>(section);
        }

        public async Task<LessionDto> GetLessionByIdAsync(string id)
        {
            var lession = await _courseCurriculumManager.GetLessionByIdAsync(id);

            if (lession == null)
            {
                throw new LessionNotFoundException(id);
            }

            return _mapper.Map<LessionDto>(lession);
        }

        public async Task<SectionDto> CreateSectionAsync(CreateSectionDto data, string courseOwnerId)
        {
            var course = await _courseRepository.FindOneAsync(data.CourseId);
            if (course == null)
            {
                throw new CourseNotFoundException(data.CourseId);
            }

            if (courseOwnerId != course.User.Id)
            {
                throw new ForbiddenException("Forbidden resourse");
            }

            var section = Section.Create(data.Title, data.CourseId, data.Description);

            await _courseCurriculumManager.CreateSectionAsync(section);
        
            return _mapper.Map<SectionDto>(section);
        }

        public async Task<LessionDto> CreateLessionAsync(CreateLessionDto data, string courseOwnerId)
        {
            var course = await _courseRepository.FindOneAsync(data.CourseId);
            if (course == null)
            {
                throw new CourseNotFoundException(data.CourseId);
            }

            if (courseOwnerId != course.User.Id)
            {
                throw new ForbiddenException("Forbidden resourse");
            }

            var section = await _courseCurriculumManager.GetSectionByIdAsync(data.SectionId);
            if (section == null || section.CourseId != course.Id)
            {
                throw new SectionNotFoundException(data.SectionId);
            }

            var lession = Lession.Create(data.Title, data.CourseId, data.SectionId, data.Description);

            await _courseCurriculumManager.CreateLessionAsync(lession);

            return _mapper.Map<LessionDto>(lession);
        }

        public async Task<CoursePublicCurriculumDto> GetPublicCurriculumAsync(string courseId)
        {
            if (!(await _courseRepository.ExistsAsync(courseId)))
            {
                throw new CourseNotFoundException(courseId);
            }

            var result = await _courseCurriculumManager.GetCourseCurriculumAsync(courseId);

            var publicSections = _mapper.Map<List<SectionInPublicCurriculumDto>>(result);

            return new CoursePublicCurriculumDto
            {
                Sections = publicSections,
            };
        }

        public async Task UpdateCurriculumAsync(string courseId, List<CurriculumItem> items, string courseOwnerId)
        {
            var course = await _courseRepository.FindOneAsync(courseId);
            if (course == null)
            {
                throw new CourseNotFoundException(courseId);
            }

            if (courseOwnerId != course.User.Id)
            {
                throw new ForbiddenException("Forbidden resourse");
            }

            await _courseCurriculumManager.UpdateCurriculumAsync(courseId, items);
        }
    }
}
