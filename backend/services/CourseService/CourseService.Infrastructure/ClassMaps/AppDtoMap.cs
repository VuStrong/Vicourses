using CourseService.Application.Dtos;
using CourseService.Application.Dtos.Category;
using MongoDB.Bson.Serialization;

namespace CourseService.Infrastructure.ClassMaps
{
    internal class AppDtoMap
    {
        public static void Configure()
        {
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

            BsonClassMap.RegisterClassMap<VideoFileDto>(map =>
            {
                map.AutoMap();

                map.SetIgnoreExtraElements(true);
            });
        }
    }
}
