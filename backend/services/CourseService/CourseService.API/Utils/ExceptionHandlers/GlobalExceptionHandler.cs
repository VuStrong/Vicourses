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

            if (exception is NotFoundException)
            {
                responseDto = new FailedResponseDto(exception.Message, 404);
                httpContext.Response.StatusCode = 404;
            }
            else if (exception is BadRequestException || exception is DomainValidationException)
            {
                responseDto = new FailedResponseDto(exception.Message, 400);
                httpContext.Response.StatusCode = 400;
            }
            else if (exception is ForbiddenException)
            {
                responseDto = new FailedResponseDto(exception.Message, 403);
                httpContext.Response.StatusCode = 403;
            }
            else if (exception is InternalServerException || exception is DomainException)
            {
                responseDto = new FailedResponseDto(exception.Message, 500);
                httpContext.Response.StatusCode = 500;
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
