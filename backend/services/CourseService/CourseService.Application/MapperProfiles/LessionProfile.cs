using AutoMapper;
using CourseService.Application.Dtos.Lession;
using CourseService.Domain.Models;

namespace CourseService.Application.MapperProfiles
{
    public class LessionProfile : Profile
    {
        public LessionProfile()
        {
            CreateMap<Lession, LessionDto>();
        }
    }
}
