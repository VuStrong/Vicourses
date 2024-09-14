using CourseService.Domain.Models;
using MongoDB.Bson.Serialization;

namespace CourseService.Infrastructure.ClassMaps
{
    public class SectionMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Section>(map =>
            {
                map.AutoMap();
                map.MapIdMember(c => c.Id);

                map.SetIgnoreExtraElements(true);
            });
        }
    }
}
