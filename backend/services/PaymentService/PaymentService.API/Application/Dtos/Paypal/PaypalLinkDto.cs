namespace PaymentService.API.Application.Dtos.Paypal
{
    public class PaypalLinkDto
    {
        public string Href { get; set; } = string.Empty;
        public string Rel { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
    }
}
