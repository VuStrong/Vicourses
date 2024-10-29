using CourseService.Application.Dtos.Category;
using CourseService.Domain.Models;
using MongoDB.Bson.Serialization;

namespace CourseService.Infrastructure.ClassMaps
{
    public class CategoryMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Category>(map =>
            {
                map.AutoMap();
                map.MapIdMember(c => c.Id);

                map.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<CategoryWithSubsDto>(map =>
            {
                map.AutoMap();
                map.MapIdMember(c => c.Id);

                map.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<CategoryDto>(map =>
            {
                map.AutoMap();
                map.MapIdMember(c => c.Id);

                map.SetIgnoreExtraElements(true);
            });
        }
    }
}
