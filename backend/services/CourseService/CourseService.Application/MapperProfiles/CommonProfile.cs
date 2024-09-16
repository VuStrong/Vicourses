using AutoMapper;
using CourseService.Shared.Paging;

namespace CourseService.Application.MapperProfiles
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));
        }
    }
}
