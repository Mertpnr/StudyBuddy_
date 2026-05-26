using StudyBuddy.API.DTOs.ChatDto;
using StudyBuddy.API.Requests.ChatRequest;

namespace StudyBuddy.API.Services.Interface
{
    public interface IChatService
    {
        Task<List<ChatListDto>> GetMessagesAsync(int matchRequestId, int userId);
        Task<int?> CreateMessageAsync(ChatCreateRequest request);
        Task<bool> UpdateMessageAsync(ChatUpdateRequest request);
        Task<bool> DeleteMessageAsync(int chatId);
    }
}