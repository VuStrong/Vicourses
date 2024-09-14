using CourseService.Domain.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace CourseService.Infrastructure.ClassMaps
{
    public class EnrollmentMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Enrollment>(map =>
            {
                map.AutoMap();
                map.MapIdMember(c => c.Id)
                    .SetIdGenerator(new StringObjectIdGenerator())
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));

                map.SetIgnoreExtraElements(true);
            });
        }
    }
}
