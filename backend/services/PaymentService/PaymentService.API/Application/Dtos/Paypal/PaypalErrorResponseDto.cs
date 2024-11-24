namespace PaymentService.API.Application.Dtos.Paypal
{
    public class PaypalErrorResponseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public List<PaypalErrorDetailResponseDto> Details { get; set; } = [];
    }

    public class PaypalErrorDetailResponseDto
    {
        public string Issue { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
