using Microsoft.Extensions.Options;
using PaymentService.API.Application.Configurations;
using PaymentService.API.Application.Dtos.Paypal;
using PaymentService.API.Application.Exceptions;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;
using System.Text;

namespace PaymentService.API.Application.Services.Paypal
{
    public class PaypalPaymentsService : BasePaypalService, IPaypalPaymentsService
    {
        public PaypalPaymentsService(IOptions<PaypalConfiguration> options) : base(options)
        {
        }

        public async Task RefundCapturedPaymentAsync(string captureId)
        {
            var accessToken = await GetAccessTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var content = new StringContent("", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"v2/payments/captures/{captureId}/refund", content);

            var json = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.Created)
            {
                var paypalErrorData = JsonSerializer.Deserialize<PaypalErrorResponseDto>(json, _jsonSerializerOptions);

                if (paypalErrorData?.Details?.Count > 0)
                {
                    var detail = paypalErrorData.Details[0];
                    throw new PaypalException(detail.Issue, detail.Description);
                }

                throw new Exception($"Cannot refund paypal captured payment: {json}");
            }
        }
    }
}
