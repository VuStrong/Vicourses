using Microsoft.AspNetCore.Diagnostics;
using StatisticsService.API.Application.Exceptions;
using StatisticsService.API.Responses;

namespace StatisticsService.API.Utils.ExceptionHandlers
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

            if (exception is AppException appException)
            {
                responseDto = new FailedResponse(appException.Message, appException.StatusCode);
                httpContext.Response.StatusCode = appException.StatusCode;
            }
            else
            {
                _logger.LogError($"[Statistics Service] An error has occured: {exception.Message}");

                responseDto = new FailedResponse("Internal server error!", 500);
                httpContext.Response.StatusCode = 500;
            }

            await httpContext.Response.WriteAsJsonAsync(responseDto, cancellationToken: cancellationToken);

            return true;
        }
    }
}
