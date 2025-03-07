namespace QuizAppWeb.Service
{
    public interface IApiService
    {
        string UserRole { get; }
        string LangPref { get; }

        Task<HttpResponseMessage> LoginAsync<T>(string url,T loginModel);
        Task<HttpResponseMessage> RegisterAsync<T>(string url, T registerModel);
        Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string> queryParams = null);
        Task<HttpResponseMessage> PostAsync<T>(string url, T body);
    }
}
