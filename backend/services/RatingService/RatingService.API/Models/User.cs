namespace RatingService.API.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? ThumbnailUrl { get; set; }

        public User(string id, string name, string email, string? thumbnailUrl)
        {
            Id = id;
            Name = name;
            Email = email;
            ThumbnailUrl = thumbnailUrl;
        }
    }
}
