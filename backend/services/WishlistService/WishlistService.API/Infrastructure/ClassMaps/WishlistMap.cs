using MongoDB.Bson.Serialization;
using WishlistService.API.Models;

namespace WishlistService.API.Infrastructure.ClassMaps
{
    public static class WishlistMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Wishlist>(map =>
            {
                map.AutoMap();
                map.MapIdMember(x => x.Id);
                map.MapField("_courses").SetElementName(nameof(Wishlist.Courses));
                map.MapField("_enrolledCourseIds").SetElementName(nameof(Wishlist.EnrolledCourseIds));

                map.SetIgnoreExtraElements(true);
            });
        }
    }
}
