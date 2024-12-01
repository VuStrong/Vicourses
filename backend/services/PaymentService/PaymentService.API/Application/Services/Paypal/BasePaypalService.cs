using Microsoft.Extensions.Options;
using PaymentService.API.Application.Configurations;
using PaymentService.API.Application.Dtos.Paypal;
using PaymentService.API.Application.Exceptions;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace PaymentService.API.Application.Services.Paypal
{
    public class BasePaypalService
    {
        private readonly PaypalConfiguration _configuration;
        protected readonly HttpClient _httpClient;

        protected readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        public BasePaypalService(IOptions<PaypalConfiguration> options)
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
    }
}
