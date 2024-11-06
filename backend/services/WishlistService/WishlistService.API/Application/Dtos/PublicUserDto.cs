namespace WishlistService.API.Application.Dtos
{
    public class PublicUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
    }
}
