namespace PaymentService.API.Application.Dtos.Paypal
{
    public class PaypalPurchaseUnitDto
    {
        public string? ReferenceId { get; set; }
        public PaypalPurchaseUnitAmountDto? Amount { get; set; }
        public PaypalPurchaseUnitPaymentsDto? Payments { get; set; }
    }

    public class PaypalPurchaseUnitAmountDto
    {
        public string CurrencyCode { get; set; } = "USD";
        public string Value { get; set; } = "0";
    }

    public class PaypalPurchaseUnitPaymentsDto
    {
        public List<PaypalPurchaseUnitPaymentsCaptureDto> Captures { get; set; } = [];
    }

    public class PaypalPurchaseUnitPaymentsCaptureDto
    {
        public string Id { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public PaypalPurchaseUnitAmountDto? Amount { get; set; }
    }
}
