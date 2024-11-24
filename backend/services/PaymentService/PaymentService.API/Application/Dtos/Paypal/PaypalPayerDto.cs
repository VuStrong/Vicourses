namespace PaymentService.API.Application.Dtos.Paypal
{
    public class PaypalPayerDto
    {
        public string PayerId { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public PaypalPayerName Name { get; set; } = null!;
    }

    public class PaypalPayerName
    {
        public string Surname { get; set; } = string.Empty;
        public string GivenName { get; set; } = string.Empty;
    }
}
