namespace CourseService.Application.Dtos
{
    public class ValidationErrorResponseDto : FailedResponseDto
    {
        public List<string> Errors { get; set; }
    
        public ValidationErrorResponseDto(List<string> errors) : base("Validation failed", 400)
        {
            Errors = errors;
        }
    }
}