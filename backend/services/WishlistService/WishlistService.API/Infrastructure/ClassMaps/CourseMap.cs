using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using WishlistService.API.Models;

namespace WishlistService.API.Infrastructure.ClassMaps
{
    public static class CourseMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Course>(map =>
            {
                map.AutoMap();
                map.MapIdMember(x => x.Id);
                map.MapMember(c => c.Price)
                    .SetSerializer(new DecimalSerializer(BsonType.Decimal128));
                map.MapMember(c => c.Rating)
                    .SetSerializer(new DecimalSerializer(BsonType.Decimal128));
                map.MapMember(c => c.Status)
                    .SetSerializer(new EnumSerializer<CourseStatus>(BsonType.String));

                map.SetIgnoreExtraElements(true);
            });
        }
    }
}
