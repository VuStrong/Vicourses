using CourseService.Domain.Models;
using MongoDB.Bson.Serialization;

namespace CourseService.Infrastructure.ClassMaps
{
    public class QuizMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Quiz>(map =>
            {
                map.AutoMap();
                map.MapIdMember(c => c.Id);
                map.MapField("_answers").SetElementName(nameof(Quiz.Answers));

                map.SetIgnoreExtraElements(true);
            });
        }
    }
}
