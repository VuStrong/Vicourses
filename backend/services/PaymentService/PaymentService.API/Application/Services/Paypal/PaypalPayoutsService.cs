using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PaymentService.API.Application.Dtos.Paypal;
using PaymentService.API.Application.Exceptions;
using PaymentService.API.Application.Configurations;
using PaymentService.API.Application.Dtos.Paypal.Payouts;

namespace PaymentService.API.Application.Services.Paypal
{
    public class PaypalPayoutsService : BasePaypalService, IPaypalPayoutsService
    {
        public PaypalPayoutsService(IOptions<PaypalConfiguration> options) : base(options)
        {
        }

        public async Task<string> BatchPayoutAsync(CreateBatchPayoutDto payload)
        {
            var accessToken = await GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.PostAsJsonAsync("v1/payments/payouts", payload, _jsonSerializerOptions);
            var json = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.Created)
            {
                var paypalErrorData = JsonSerializer.Deserialize<PaypalErrorResponseDto>(json, _jsonSerializerOptions);

                if (paypalErrorData?.Details?.Count > 0)
                {
                    var detail = paypalErrorData.Details[0];
                    throw new PaypalException(detail.Issue, detail.Description);
                }

                throw new Exception($"Cannot create batch payout: {json}");
            }

            var data = JsonDocument.Parse(json);
            var id = data.RootElement
                .GetProperty("batch_header")
                .GetProperty("payout_batch_id")
                .GetString();

            return id!;
        }

        public async Task<object> GetBatchPayoutAsync(string id, int page = 0, int pageSize = 20)
        {
            var accessToken = await GetAccessTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync($"v1/payments/payouts/{id}");

            var data = await response.Content.ReadFromJsonAsync<object>();

            return data!;
        }
    }
}
