using StudyBuddy.WEB.Models.Chat;
using StudyBuddy.WEB.Services.Interfaces;

namespace StudyBuddy.WEB.Services
{
    public class ChatWebService : IChatWebService
    {
        private readonly IApiClientService _apiClientService;

        public ChatWebService(IApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public async Task<List<ChatListViewModel>> GetMessagesAsync(int matchRequestId, int userId)
        {
            return await _apiClientService.GetAsync<List<ChatListViewModel>>(
                $"Chat/GetMessages/{matchRequestId}/{userId}"
            ) ?? new List<ChatListViewModel>();
        }

        public async Task<bool> SendMessageAsync(ChatCreateViewModel model)
        {
            return await _apiClientService.PostAsync("Chat/Create", model);
        }
    }
}