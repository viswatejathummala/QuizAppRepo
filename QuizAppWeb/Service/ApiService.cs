using QuizAppWeb.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace QuizAppWeb.Service
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private IHttpContextAccessor _httpContextAccessor;

        public ApiService(HttpClient httpClient, IConfiguration configuration,IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:Url"];
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HttpResponseMessage> LoginAsync<T>(string endpoint,T loginModel)
        {
            try
            {
                var url = $"{_baseUrl}/{endpoint}";
                var response = await _httpClient.PostAsJsonAsync(url, loginModel);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                   
                    var token = JsonSerializer.Deserialize<JWTResponse>(result).Token;

                    _httpContextAccessor.HttpContext.Session.SetString("jwtToken", token);

                }

                return response;
            }
            catch (Exception ex)
            {
                // Handle any exceptions during login
                throw new InvalidOperationException("Error during login", ex);
            }
        }
        public async Task<HttpResponseMessage> RegisterAsync<T>(string endpoint, T registerModel)
        {
            try
            {
                var url = $"{_baseUrl}/{endpoint}";
                var response = await _httpClient.PostAsJsonAsync(url, registerModel);

                return response; // Return the HTTP response
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error during registration", ex);
            }
        }
        private void AddAuthorizationHeader()
        {
            string token = _httpContextAccessor.HttpContext.Session.GetString("jwtToken");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
        // GET Request - Handles query parameters
        public async Task<HttpResponseMessage> GetAsync(string endpoint, Dictionary<string, string> queryParams = null)
        {
            AddAuthorizationHeader();

            var url = $"{_baseUrl}/{endpoint}"; 
            if (queryParams != null && queryParams.Any())
            {
                // Add query parameters to the URL
                var queryString = await new FormUrlEncodedContent(queryParams).ReadAsStringAsync();
                url = $"{url}?{queryString}";
            }

            var response = await _httpClient.GetAsync(url);
            return response;
        }

        // POST Request - Sends data in the body
        public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T body)
        {
            AddAuthorizationHeader();

            var url = $"{_baseUrl}/{endpoint}";  
            var response = await _httpClient.PostAsJsonAsync(url, body);
            return response;
        }

    }
}
