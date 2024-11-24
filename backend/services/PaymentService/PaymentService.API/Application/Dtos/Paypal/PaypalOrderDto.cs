namespace PaymentService.API.Application.Dtos.Paypal
{
    public class PaypalOrderDto
    {
        public string Id { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<PaypalPurchaseUnitDto> PurchaseUnits { get; set; } = [];
        public PaypalPayerDto Payer { get; set; } = null!;
        public List<PaypalLinkDto> Links { get; set; } = [];
    }
}
