using AutoMapper;
using WishlistService.API.Application.Dtos;
using WishlistService.API.Application.IntegrationEvents.Course;
using WishlistService.API.Models;

namespace WishlistService.API.Application.MapperProfiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<PublicUserDto, UserInCourse>();
            CreateMap<UserInCourse, PublicUserDto>();

            CreateMap<CourseInfoUpdatedIntegrationEvent, Course>();
            CreateMap<CoursePublishedIntegrationEvent, Course>();

            CreateMap<Course, CourseDto>();
            CreateMap<Wishlist, WishlistDto>();
        }
    }
}
