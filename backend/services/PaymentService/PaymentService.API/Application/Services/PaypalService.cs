using Microsoft.Extensions.Options;
using PaymentService.API.Application.Configurations;
using PaymentService.API.Application.Dtos.Paypal;
using PaymentService.API.Application.Exceptions;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PaymentService.API.Application.Services
{
    public class PaypalService : IPaypalService
    {
        private readonly PaypalConfiguration _configuration;
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        public PaypalService(IOptions<PaypalConfiguration> options)
        {
            _configuration = options.Value;

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_configuration.Base);
            _httpClient.DefaultRequestHeaders
              .Accept
              .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var authBytes = Encoding.ASCII.GetBytes($"{_configuration.ClientId}:{_configuration.ClientSecret}");
            var authString = Convert.ToBase64String(authBytes);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authString);

            var body = new StringContent("grant_type=client_credentials");
            var response = await _httpClient.PostAsync("/v1/oauth2/token", body);

            var json = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var paypalErrorData = JsonSerializer.Deserialize<PaypalErrorResponseDto>(json, _jsonSerializerOptions);

                if (paypalErrorData?.Details.Count > 0)
                {
                    var detail = paypalErrorData.Details[0];
                    throw new PaypalException(detail.Issue, detail.Description);
                }

                throw new Exception($"Cannot get paypal access token: {json}");
            }

            var data = JsonDocument.Parse(json);
            var accessToken = data.RootElement.GetProperty("access_token").GetString();

            return accessToken ?? "";
        }

        public async Task<PaypalOrderDto> GetOrderAsync(string orderId)
        {
            var accessToken = await GetAccessTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync($"/v2/checkout/orders/{orderId}");

            var json = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var paypalErrorData = JsonSerializer.Deserialize<PaypalErrorResponseDto>(json, _jsonSerializerOptions);

                if (paypalErrorData?.Details.Count > 0)
                {
                    var detail = paypalErrorData.Details[0];
                    throw new PaypalException(detail.Issue, detail.Description);
                }

                throw new Exception($"Cannot get paypal order: {json}");
            }

            var paypalOrder = JsonSerializer.Deserialize<PaypalOrderDto>(json, _jsonSerializerOptions);

            return paypalOrder!;
        }

        public async Task<PaypalOrderDto> CreateOrderAsync(CreatePaypalOrderDto payload)
        {
            var accessToken = await GetAccessTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.PostAsJsonAsync("/v2/checkout/orders", payload, _jsonSerializerOptions);

            var json = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.Created)
            {
                var paypalErrorData = JsonSerializer.Deserialize<PaypalErrorResponseDto>(json, _jsonSerializerOptions);

                if (paypalErrorData?.Details.Count > 0)
                {
                    var detail = paypalErrorData.Details[0];
                    throw new PaypalException(detail.Issue, detail.Description);
                }

                throw new Exception($"Cannot create paypal order: {json}");
            }

            var paypalOrder = JsonSerializer.Deserialize<PaypalOrderDto>(json, _jsonSerializerOptions);

            return paypalOrder!;
        }

        public async Task<PaypalOrderDto> CaptureOrderAsync(string orderId)
        {
            var accessToken = await GetAccessTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var content = new StringContent("", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"/v2/checkout/orders/{orderId}/capture", content);

            var json = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.Created)
            {
                var paypalErrorData = JsonSerializer.Deserialize<PaypalErrorResponseDto>(json, _jsonSerializerOptions);

                if (paypalErrorData?.Details.Count > 0)
                {
                    var detail = paypalErrorData.Details[0];
                    throw new PaypalException(detail.Issue, detail.Description);
                }

                throw new Exception($"Cannot capture paypal order: {json}");
            }

            var capturedPaypalOrder = JsonSerializer.Deserialize<PaypalOrderDto>(json, _jsonSerializerOptions);

            return capturedPaypalOrder!;
        }
    }
}
