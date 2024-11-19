namespace StatisticsService.API.Models
{
    public class User
    {
        public string Id { get; set; }
        public DateOnly CreatedAt { get; set; }
        public string Role { get; set; }

        public User(string id, string role)
        {
            Id = id;
            CreatedAt = DateOnly.FromDateTime(DateTime.Today);
            Role = role;
        }
    }
}
