namespace WishlistService.API.Application.Dtos
{
    public class AddToWishlistDto
    {
        public required string UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public required string CourseId { get; set; }
    }
}
