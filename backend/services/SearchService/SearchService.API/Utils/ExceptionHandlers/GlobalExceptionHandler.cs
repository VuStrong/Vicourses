using Microsoft.AspNetCore.Diagnostics;
using SearchService.API.Application.Dtos;
using SearchService.API.Application.Exceptions;

namespace SearchService.API.Utils.ExceptionHandlers
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
            FailedResponseDto? responseDto;

            if (exception is NotFoundException)
            {
                responseDto = new FailedResponseDto(exception.Message, 404);
                httpContext.Response.StatusCode = 404;
            }
            else
            {
                _logger.LogError($"[Search Service] An error has occured: {exception.Message}");

                responseDto = new FailedResponseDto("Internal server error!", 500);
                httpContext.Response.StatusCode = 500;
            }

            await httpContext.Response.WriteAsJsonAsync(responseDto, cancellationToken: cancellationToken);

            return true;
        }
    }
}