using Microsoft.AspNetCore.Diagnostics;
using WishlistService.API.Application.Dtos;
using WishlistService.API.Application.Exceptions;

namespace WishlistService.API.Utils.ExceptionHandlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            FailedResponse? responseDto;

            if (exception is NotFoundException)
            {
                responseDto = new FailedResponse(exception.Message, 404);
                httpContext.Response.StatusCode = 404;
            }
            else if (exception is ForbiddenException)
            {
                responseDto = new FailedResponse(exception.Message, 403);
                httpContext.Response.StatusCode = 403;
            }
            else
            {
                _logger.LogError($"[Wishlist Service] An error has occured: {exception.Message}");

                responseDto = new FailedResponse("Internal server error!", 500);
                httpContext.Response.StatusCode = 500;
            }

            await httpContext.Response.WriteAsJsonAsync(responseDto, cancellationToken: cancellationToken);

            return true;
        }
    }
}
