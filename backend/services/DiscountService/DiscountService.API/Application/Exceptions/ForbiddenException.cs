namespace DiscountService.API.Application.Exceptions
{
    public class ForbiddenException : AppException
    {
        public ForbiddenException(string message) : base(message, 403)
        {
        }
    }
}