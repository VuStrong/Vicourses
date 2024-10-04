using AutoMapper;
using CourseService.Application.Dtos;
using CourseService.Domain.Objects;
using CourseService.Shared.Paging;

namespace CourseService.Application.MapperProfiles
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));

            CreateMap<VideoFile, VideoFileDto>();

            CreateMap<Locale, LocaleDto>();
        }
    }
}
