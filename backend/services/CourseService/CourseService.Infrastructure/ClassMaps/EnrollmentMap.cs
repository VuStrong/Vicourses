using CourseService.Domain.Models;
using MongoDB.Bson.Serialization;

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

                map.SetIgnoreExtraElements(true);
            });
        }
    }
}
