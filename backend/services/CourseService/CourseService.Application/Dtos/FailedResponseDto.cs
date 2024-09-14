namespace CourseService.Application.Dtos
{
    public class FailedResponseDto
    {
        public string Message { get; set; }
        public int Code { get; set; }

        public FailedResponseDto(string message, int code)
        {
            Message = message;
            Code = code;
        }
    }
}
