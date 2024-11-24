namespace PaymentService.API.Responses
{
    public class FailedResponse
    {
        public string Message { get; set; }
        public int Code { get; set; }

        public FailedResponse(string message, int code)
        {
            Message = message;
            Code = code;
        }
    }
}