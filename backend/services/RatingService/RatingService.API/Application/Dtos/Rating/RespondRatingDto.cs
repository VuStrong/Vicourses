namespace RatingService.API.Application.Dtos.Rating
{
    public class RespondRatingDto
    {
        public string UserId { get; set; }
        public string Response { get; set; }

        public RespondRatingDto(string userId, string response)
        {
            UserId = userId;
            Response = response;
        }
    }
}
