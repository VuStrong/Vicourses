using AutoMapper;
using CourseService.Application.Dtos.Course;
using CourseService.Domain.Models;

namespace CourseService.Application.MapperProfiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CategoryInCourse, CategoryInCourseDto>();
            CreateMap<UserInCourse, UserInCourseDto>();

            CreateMap<Course, CourseDto>()
                .ForMember(
                    dest => dest.ThumbnailUrl, 
                    opt => opt.MapFrom(src => src.Thumbnail != null ? src.Thumbnail.Url : null));

            CreateMap<Course, CourseDetailDto>()
                .ForMember(
                    dest => dest.ThumbnailUrl,
                    opt => opt.MapFrom(src => src.Thumbnail != null ? src.Thumbnail.Url : null));
        }
    }
}
