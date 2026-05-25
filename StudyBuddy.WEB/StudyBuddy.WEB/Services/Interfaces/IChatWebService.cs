using StudyBuddy.WEB.Models.Chat;

namespace StudyBuddy.WEB.Services.Interfaces
{
    public interface IChatWebService
    {
        Task<List<ChatListViewModel>> GetMessagesAsync(int matchRequestId, int userId);
        Task<bool> SendMessageAsync(ChatCreateViewModel model);
    }
}