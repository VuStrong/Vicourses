using CourseService.Domain.Enums;
using CourseService.Domain.Objects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace CourseService.Infrastructure.ClassMaps
{
    internal class VideoFileMap
    {
        public static void Congifure()
        {
            BsonClassMap.RegisterClassMap<VideoFile>(map =>
            {
                map.AutoMap();
                map.MapMember(c => c.Status)
                    .SetSerializer(new EnumSerializer<VideoStatus>(BsonType.String));

                map.SetIgnoreExtraElements(true);
            });
        }
    }
}
