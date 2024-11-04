using AutoMapper;
using RatingService.API.Application.Dtos;
using RatingService.API.Models;

namespace RatingService.API.Application.MapperProfiles
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));
            CreateMap<User, PublicUserDto>();
        }
    }
}
