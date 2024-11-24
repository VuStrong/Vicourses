namespace PaymentService.API.Application.Exceptions
{
    public class PaypalException : Exception
    {
        public string Issue { get; set; }

        public PaypalException(string issue, string? message) : base(message)
        {
            Issue = issue;
        }
    }
}
