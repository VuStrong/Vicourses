using CourseService.Domain.Models;
using MongoDB.Bson.Serialization;

namespace CourseService.Infrastructure.ClassMaps
{
    internal class CommentMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Comment>(map =>
            {
                map.AutoMap();
                map.MapIdMember(c => c.Id);
                map.MapField("_userUpvoteIds").SetElementName(nameof(Comment.UserUpvoteIds));

                map.SetIgnoreExtraElements(true);
            });
        }
    }
}
