using AutoMapper;
using CourseService.Application.Dtos.Course;
using CourseService.Application.Dtos.Lesson;
using CourseService.Application.Dtos.Section;
using CourseService.Application.Exceptions;
using CourseService.Application.Interfaces;
using CourseService.Domain.Contracts;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Lesson;
using CourseService.Domain.Events.Section;
using CourseService.Domain.Models;
using CourseService.Domain.Objects;
using Microsoft.Extensions.Logging;

namespace CourseService.Application.Services
{
    public class CourseCurriculumService : ICourseCurriculumService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly ICourseCurriculumRepository _courseCurriculumRepository;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly IFileUploadTokenValidator _fileUploadTokenValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<CourseCurriculumService> _logger;

        public CourseCurriculumService(
            ICourseRepository courseRepository,
            ISectionRepository sectionRepository,
            ILessonRepository lessonRepository,
            ICourseCurriculumRepository courseCurriculumRepository,
            IDomainEventDispatcher domainEventDispatcher,
            IFileUploadTokenValidator fileUploadTokenValidator,
            IMapper mapper,
            ILogger<CourseCurriculumService> logger)
        {
            _courseRepository = courseRepository;
            _sectionRepository = sectionRepository;
            _lessonRepository = lessonRepository;
            _courseCurriculumRepository = courseCurriculumRepository;
            _domainEventDispatcher = domainEventDispatcher;
            _fileUploadTokenValidator = fileUploadTokenValidator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SectionDto> GetSectionByIdAsync(string id)
        {
            var section = await _sectionRepository.FindOneAsync(id);

            if (section == null)
            {
                throw new SectionNotFoundException(id);
            }

            return _mapper.Map<SectionDto>(section);
        }

        public async Task<LessonDto> GetLessonByIdAsync(string id)
        {
            var lesson = await _lessonRepository.FindOneAsync(id);

            if (lesson == null)
            {
                throw new LessonNotFoundException(id);
            }

            return _mapper.Map<LessonDto>(lesson);
        }

        public async Task<SectionDto> CreateSectionAsync(CreateSectionDto data)
        {
            var course = await _courseRepository.FindOneAsync(data.CourseId);
            if (course == null)
            {
                throw new CourseNotFoundException(data.CourseId);
            }

            if (data.UserId != course.User.Id)
            {
                throw new ForbiddenException("Forbidden resourse");
            }

            var section = Section.Create(data.Title, course, data.Description);

            await _sectionRepository.CreateAsync(section);
        
            return _mapper.Map<SectionDto>(section);
        }

        public async Task<SectionDto> UpdateSectionAsync(string sectionId, UpdateSectionDto data, string ownerId)
        {
            var section = await _sectionRepository.FindOneAsync(sectionId);

            if (section == null)
            {
                throw new SectionNotFoundException(sectionId);
            }

            if (section.UserId != ownerId)
            {
                throw new ForbiddenException("Forbidden resource");
            }

            section.UpdateInfoIgnoreNull(data.Title, data.Description);

            await _sectionRepository.UpdateAsync(section);

            return _mapper.Map<SectionDto>(section);
        }

        public async Task DeleteSectionAsync(string sectionId, string ownerId)
        {
            var section = await _sectionRepository.FindOneAsync(sectionId);

            if (section == null)
            {
                throw new SectionNotFoundException(sectionId);
            }

            if (section.UserId != ownerId)
            {
                throw new ForbiddenException("Forbidden resource");
            }

            await _sectionRepository.DeleteOneAsync(sectionId);

            _ = _domainEventDispatcher.Dispatch(new SectionDeletedDomainEvent(section));
        }

        public async Task<LessonDto> CreateLessonAsync(CreateLessonDto data)
        {
            var course = await _courseRepository.FindOneAsync(data.CourseId);
            if (course == null)
            {
                throw new CourseNotFoundException(data.CourseId);
            }

            if (data.UserId != course.User.Id)
            {
                throw new ForbiddenException("Forbidden resourse");
            }

            var section = await _sectionRepository.FindOneAsync(data.SectionId);
            if (section == null || section.CourseId != course.Id)
            {
                throw new SectionNotFoundException(data.SectionId);
            }

            var lesson = Lesson.Create(data.Title, course, section, data.Type, data.Description);

            await _lessonRepository.CreateAsync(lesson);

            return _mapper.Map<LessonDto>(lesson);
        }

        public async Task<LessonDto> UpdateLessonAsync(string lessonId, UpdateLessonDto data, string ownerId)
        {
            var lesson = await _lessonRepository.FindOneAsync(lessonId);

            if (lesson == null)
            {
                throw new LessonNotFoundException(lessonId);
            }

            if (lesson.UserId != ownerId)
            {
                throw new ForbiddenException("Forbidden resourse");
            }

            VideoFile? videoFile = null;
            if (data.VideoToken != null)
            {
                var uploadFileDto = _fileUploadTokenValidator.ValidateFileUploadToken(data.VideoToken, ownerId);

                videoFile = VideoFile.Create(uploadFileDto.FileId, uploadFileDto.Url, uploadFileDto.OriginalFileName);
            }
            
            lesson.UpdateInfoIgnoreNull(data.Title, data.Description);

            if (videoFile != null)
                lesson.UpdateVideo(videoFile);

            await _lessonRepository.UpdateAsync(lesson);

            return _mapper.Map<LessonDto>(lesson);
        }

        public async Task DeleteLessonAsync(string lessonId, string ownerId)
        {
            var lesson = await _lessonRepository.FindOneAsync(lessonId);

            if (lesson == null)
            {
                throw new LessonNotFoundException(lessonId);
            }

            if (lesson.UserId != ownerId)
            {
                throw new ForbiddenException("Forbidden resourse");
            }

            await _lessonRepository.DeleteOneAsync(lessonId);

            _ = _domainEventDispatcher.Dispatch(new LessonDeletedDomainEvent(lesson));
        }

        public async Task<CoursePublicCurriculumDto> GetPublicCurriculumAsync(string courseId)
        {
            var result = await _courseCurriculumRepository.GetCourseCurriculumAsync(courseId);

            var publicSections = _mapper.Map<List<SectionInPublicCurriculumDto>>(result);

            return new CoursePublicCurriculumDto
            {
                Sections = publicSections,
            };
        }

        public async Task<CourseInstructorCurriculumDto> GetInstructorCurriculumAsync(string courseId)
        {
            var result = await _courseCurriculumRepository.GetCourseCurriculumAsync(courseId);

            var sections = _mapper.Map<List<SectionInInstructorCurriculumDto>>(result);

            return new CourseInstructorCurriculumDto
            {
                Sections = sections,
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

            await _courseCurriculumRepository.UpdateCurriculumAsync(courseId, items);
        }
    }
}
