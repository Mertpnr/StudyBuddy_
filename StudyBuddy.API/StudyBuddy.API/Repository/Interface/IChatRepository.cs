using StudyBuddy.API.Model;

namespace StudyBuddy.API.Repository.Interface
{
    public interface IChatRepository : IGenericRepository<Chat, int>
    {
        Task<IEnumerable<Chat>> GetByMatchRequestIdAsync(int matchRequestId);
        Task<bool> UserCanAccessChatAsync(int matchRequestId, int userId);
        Task<int> AddMessageAsync(Chat chat);
        Task<bool> UpdateMessageAsync(Chat chat);
    }
}