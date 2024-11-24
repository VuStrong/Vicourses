namespace PaymentService.API.Application.Configurations
{
    public class PaypalConfiguration
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string Base { get; set; } = string.Empty;
        public string Mode { get; set; } = "sandbox";
    }
}
