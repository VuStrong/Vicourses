using CourseService.Domain.Enums;
using CourseService.Domain.Models;
using CourseService.Domain.Objects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace CourseService.Infrastructure.ClassMaps
{
    public class LessonMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Lesson>(map =>
            {
                map.AutoMap();
                map.MapIdMember(c => c.Id);
                map.MapMember(c => c.Type)
                    .SetSerializer(new EnumSerializer<LessonType>(BsonType.String));

                map.MapField("_quizzes").SetElementName(nameof(Lesson.Quizzes));

                map.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<Quiz>(map =>
            {
                map.AutoMap();

                map.MapField("_answers").SetElementName(nameof(Quiz.Answers));

                map.SetIgnoreExtraElements(true);
            });
        }
    }
}
