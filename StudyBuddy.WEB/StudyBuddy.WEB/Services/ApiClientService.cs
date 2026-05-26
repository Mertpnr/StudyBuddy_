using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using StudyBuddy.WEB.Models.Api;
using StudyBuddy.WEB.Services.Interfaces;

namespace StudyBuddy.WEB.Services
{
    public class ApiClientService : IApiClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiClientService(
            IHttpClientFactory httpClientFactory,
            IOptions<ApiSettings> apiSettings,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings.Value;
            _httpContextAccessor = httpContextAccessor;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_apiSettings.BaseUrl);

            var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

        public async Task<TResponse?> GetAsync<TResponse>(string endpoint)
        {
            var client = CreateClient();

            var response = await client.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
                return default;

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<TResponse>(content, _jsonOptions);
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var client = CreateClient();

            var json = JsonSerializer.Serialize(data);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(endpoint, httpContent);

            if (!response.IsSuccessStatusCode)
                return default;

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions);
        }

        public async Task<bool> PostAsync<TRequest>(string endpoint, TRequest data)
        {
            var client = CreateClient();

            var json = JsonSerializer.Serialize(data);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(endpoint, httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var client = CreateClient();

            var json = JsonSerializer.Serialize(data);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(endpoint, httpContent);

            if (!response.IsSuccessStatusCode)
                return default;

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions);
        }

        public async Task<bool> PutAsync<TRequest>(string endpoint, TRequest data)
        {
            var client = CreateClient();

            var json = JsonSerializer.Serialize(data);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(endpoint, httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            var client = CreateClient();

            var response = await client.DeleteAsync(endpoint);

            return response.IsSuccessStatusCode;
        }
    }
}
