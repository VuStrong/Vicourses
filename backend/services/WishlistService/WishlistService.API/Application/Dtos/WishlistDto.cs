namespace WishlistService.API.Application.Dtos
{
    public class WishlistDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int Count { get; set; }
        public List<CourseDto> Courses { get; set; } = [];
    }
}
