using AutoMapper;
using SearchService.API.Application.IntegrationEvents.Course;
using SearchService.API.Models;

namespace SearchService.API.Application.MapperProfiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CourseInfoUpdatedIntegrationEvent, Course>();
            CreateMap<CoursePublishedIntegrationEvent, Course>();
        }
    }
}
