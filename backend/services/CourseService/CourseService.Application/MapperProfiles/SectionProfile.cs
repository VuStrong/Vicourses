using AutoMapper;
using CourseService.Application.Dtos.Section;
using CourseService.Domain.Models;

namespace CourseService.Application.MapperProfiles
{
    public class SectionProfile : Profile
    {
        public SectionProfile()
        {
            CreateMap<Section, SectionDto>();
        }
    }
}
