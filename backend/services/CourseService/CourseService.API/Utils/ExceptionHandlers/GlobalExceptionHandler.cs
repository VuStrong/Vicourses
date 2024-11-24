using CourseService.Application.Dtos;
using CourseService.Application.Exceptions;
using CourseService.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace CourseService.API.Utils.ExceptionHandlers
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

            if (exception is AppException appException)
            {
                responseDto = new FailedResponseDto(appException.Message, appException.StatusCode);
                httpContext.Response.StatusCode = appException.StatusCode;
            }
            else if (exception is DomainValidationException)
            {
                responseDto = new FailedResponseDto(exception.Message, StatusCodes.Status400BadRequest);
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            else if (exception is BusinessRuleViolationException)
            {
                responseDto = new FailedResponseDto(exception.Message, StatusCodes.Status422UnprocessableEntity);
                httpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            }
            else
            {
                _logger.LogError($"[Course Service] An error has occured: {exception.Message}");

                responseDto = new FailedResponseDto("Internal server error!", 500);
                httpContext.Response.StatusCode = 500;
            }

            await httpContext.Response.WriteAsJsonAsync(responseDto, cancellationToken: cancellationToken);

            return true;
        }
    }
}
