using CourseService.Domain.Enums;
using CourseService.Domain.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace CourseService.Infrastructure.ClassMaps
{
    public class CourseMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Course>(map =>
            {
                map.AutoMap();
                map.MapIdMember(c => c.Id);
                map.MapMember(c => c.Level)
                    .SetSerializer(new EnumSerializer<CourseLevel>(BsonType.String));
                map.MapMember(c => c.Status)
                    .SetSerializer(new EnumSerializer<CourseStatus>(BsonType.String));

                map.MapMember(c => c.Price)
                    .SetSerializer(new DecimalSerializer(BsonType.Decimal128));
                map.MapMember(c => c.Rating)
                    .SetSerializer(new DecimalSerializer(BsonType.Decimal128));

                map.SetIgnoreExtraElements(true);
            });
        }
    }
}
