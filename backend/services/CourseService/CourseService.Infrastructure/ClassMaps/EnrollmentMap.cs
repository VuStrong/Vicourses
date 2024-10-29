using CourseService.Domain.Models;
using MongoDB.Bson.Serialization;
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
                map.MapIdMember(c => c.Id);
                map.MapMember(c => c.EnrolledAt)
                    .SetSerializer(new DateTimeSerializer(dateOnly: true));

                map.SetIgnoreExtraElements(true);
            });
        }
    }
}
