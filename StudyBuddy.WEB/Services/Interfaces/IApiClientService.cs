namespace StudyBuddy.WEB.Services.Interfaces
{
    public interface IApiClientService
    {
        Task<TResponse?> GetAsync<TResponse>(string endpoint);

        Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data);

        Task<bool> PostAsync<TRequest>(string endpoint, TRequest data);

        Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data);

        Task<bool> PutAsync<TRequest>(string endpoint, TRequest data);

        Task<bool> DeleteAsync(string endpoint);
    }
}
