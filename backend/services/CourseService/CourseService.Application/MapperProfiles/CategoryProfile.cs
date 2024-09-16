using AutoMapper;
using CourseService.Application.Dtos.Category;
using CourseService.Domain.Models;

namespace CourseService.Application.MapperProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>();

            CreateMap<CategoryWithSubs, CategoryWithSubsDto>();
        }
    }
}
