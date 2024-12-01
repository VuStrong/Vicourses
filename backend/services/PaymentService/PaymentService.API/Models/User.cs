namespace PaymentService.API.Models
{
    public class User
    {
        public string Id { get; private set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PaypalPayerId { get; set; }
        public string? PaypalEmail { get; set; }

        public User(string id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}
