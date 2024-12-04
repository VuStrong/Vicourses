using AutoMapper;
using CourseService.Application.Dtos.Course;
using CourseService.Application.Dtos.Lesson;
using CourseService.Application.Dtos.Section;
using CourseService.Application.IntegrationEvents.Course;
using CourseService.Domain.Models;
using CourseService.Domain.Objects;

namespace CourseService.Application.MapperProfiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CategoryInCourse, CategoryInCourseDto>();
            CreateMap<UserInCourse, UserInCourseDto>();
            CreateMap<CourseMetrics, CourseMetricsDto>();

            CreateMap<LessonDto, LessonInPublicCurriculumDto>()
                .ForMember(
                    dest => dest.Duration,
                    opt => opt.MapFrom(src => src.Video != null ? src.Video.Duration : 0)
                );
            CreateMap<LessonDto, LessonInInstructorCurriculumDto>();
            CreateMap<SectionWithLessonsDto, SectionInPublicCurriculumDto>();
            CreateMap<SectionWithLessonsDto, SectionInInstructorCurriculumDto>();

            CreateMap<Course, CourseDto>()
                .ForMember(
                    dest => dest.ThumbnailUrl, 
                    opt => opt.MapFrom(src => src.Thumbnail != null ? src.Thumbnail.Url : null));

            CreateMap<Course, CourseDetailDto>()
                .ForMember(
                    dest => dest.ThumbnailUrl,
                    opt => opt.MapFrom(src => src.Thumbnail != null ? src.Thumbnail.Url : null));

            CreateMap<Course, CoursePublishedIntegrationEvent>()
                .ForMember(
                    dest => dest.ThumbnailUrl,
                    opt => opt.MapFrom(src => src.Thumbnail != null ? src.Thumbnail.Url : null));

            CreateMap<Course, CourseInfoUpdatedIntegrationEvent>()
                .ForMember(
                    dest => dest.ThumbnailUrl,
                    opt => opt.MapFrom(src => src.Thumbnail != null ? src.Thumbnail.Url : null));
        }
    }
}
