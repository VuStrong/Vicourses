using AutoMapper;
using RatingService.API.Application.Dtos.Rating;
using RatingService.API.Models;

namespace RatingService.API.Application.MapperProfiles
{
    public class RatingProfile : Profile
    {
        public RatingProfile()
        {
            CreateMap<Rating, RatingDto>();
        }
    }
}
