namespace PaymentService.API.Responses
{
    public class ValidationErrorResponse : FailedResponse
    {
        public List<string> Errors { get; set; }

        public ValidationErrorResponse(List<string> errors) : base("Validation failed", 400)
        {
            Errors = errors;
        }
    }
}