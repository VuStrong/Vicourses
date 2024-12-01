namespace PaymentService.API.Application.Dtos.Paypal.Payouts
{
    public class CreateBatchPayoutDto
    {
        public List<PayoutItemDto> Items { get; set; } = [];
        public required PayoutSenderBatchHeaderDto SenderBatchHeader { get; set; }
    }

    public class PayoutItemDto
    {
        public string RecipientType { get; set; } = "PAYPAL_ID";
        public required PayoutItemAmountDto Amount { get; set; }
        public required string Receiver { get; set; }
        public string Purpose { get; set; } = "AWARDS";
    }

    public class PayoutItemAmountDto
    {
        public string Value { get; set; } = "0";
        public string Currency { get; set; } = "currency";
    }

    public class PayoutSenderBatchHeaderDto
    {
        public string RecipientType { get; set; } = "PAYPAL_ID";
        public string EmailSubject { get; set; } = string.Empty;
        public string EmailMessage { get; set; } = string.Empty;
    }
}
