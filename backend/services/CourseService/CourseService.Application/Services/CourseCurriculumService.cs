﻿using AutoMapper;
using CourseService.Application.Dtos.Course;
using CourseService.Application.Dtos.Lession;
using CourseService.Application.Dtos.Section;
using CourseService.Application.Exceptions;
using CourseService.Application.Interfaces;
using CourseService.Domain.Contracts;
using CourseService.Domain.Models;
using Microsoft.Extensions.Logging;

namespace CourseService.Application.Services
{
    public class CourseCurriculumService : ICourseCurriculumService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseCurriculumRepository _courseCurriculumRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CourseCurriculumService> _logger;

        public CourseCurriculumService(
            ICourseRepository courseRepository,
            ICourseCurriculumRepository courseCurriculumRepository,
            IMapper mapper,
            ILogger<CourseCurriculumService> logger)
        {
            _courseRepository = courseRepository;
            _courseCurriculumRepository = courseCurriculumRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SectionDto> GetSectionByIdAsync(string id)
        {
            var section = await _courseCurriculumRepository.GetSectionByIdAsync(id);

            if (section == null)
            {
                throw new SectionNotFoundException(id);
            }

            return _mapper.Map<SectionDto>(section);
        }

        public async Task<LessionDto> GetLessionByIdAsync(string id)
        {
            var lession = await _courseCurriculumRepository.GetLessionByIdAsync(id);

            if (lession == null)
            {
                throw new LessionNotFoundException(id);
            }

            return _mapper.Map<LessionDto>(lession);
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

            var section = Section.Create(data.Title, data.CourseId, data.UserId, data.Description);

            await _courseCurriculumRepository.CreateSectionAsync(section);
        
            return _mapper.Map<SectionDto>(section);
        }

        public async Task<SectionDto> UpdateSectionAsync(string sectionId, UpdateSectionDto data, string ownerId)
        {
            var section = await _courseCurriculumRepository.GetSectionByIdAsync(sectionId);

            if (section == null)
            {
                throw new SectionNotFoundException(sectionId);
            }

            if (section.UserId != ownerId)
            {
                throw new ForbiddenException("Forbidden resource");
            }

            section.UpdateInfoIgnoreNull(data.Title, data.Description);

            await _courseCurriculumRepository.UpdateSectionAsync(section);

            return _mapper.Map<SectionDto>(section);
        }

        public async Task DeleteSectionAsync(string sectionId, string ownerId)
        {
            var section = await _courseCurriculumRepository.GetSectionByIdAsync(sectionId);

            if (section == null)
            {
                throw new SectionNotFoundException(sectionId);
            }

            if (section.UserId != ownerId)
            {
                throw new ForbiddenException("Forbidden resource");
            }

            await _courseCurriculumRepository.DeleteSectionAsync(sectionId);
        }

        public async Task<LessionDto> CreateLessionAsync(CreateLessionDto data)
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

            var section = await _courseCurriculumRepository.GetSectionByIdAsync(data.SectionId);
            if (section == null || section.CourseId != course.Id)
            {
                throw new SectionNotFoundException(data.SectionId);
            }

            var lession = Lession.Create(data.Title, course, section, data.UserId, data.Type, data.Description);

            await _courseCurriculumRepository.CreateLessionAsync(lession);

            return _mapper.Map<LessionDto>(lession);
        }

        public async Task<LessionDto> UpdateLessionAsync(string lessionId, UpdateLessionDto data, string ownerId)
        {
            var lession = await _courseCurriculumRepository.GetLessionByIdAsync(lessionId);

            if (lession == null)
            {
                throw new LessionNotFoundException(lessionId);
            }

            if (lession.UserId != ownerId)
            {
                throw new ForbiddenException("Forbidden resourse");
            }

            lession.UpdateInfoIgnoreNull(data.Title, data.Description);

            await _courseCurriculumRepository.UpdateLessionAsync(lession);

            return _mapper.Map<LessionDto>(lession);
        }

        public async Task DeleteLessionAsync(string lessionId, string ownerId)
        {
            var lession = await _courseCurriculumRepository.GetLessionByIdAsync(lessionId);

            if (lession == null)
            {
                throw new LessionNotFoundException(lessionId);
            }

            if (lession.UserId != ownerId)
            {
                throw new ForbiddenException("Forbidden resourse");
            }

            await _courseCurriculumRepository.DeleteLessionAsync(lessionId);
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
