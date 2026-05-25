using StudyBuddy.WEB.Models.MatchRequest;

namespace StudyBuddy.WEB.Services.Interfaces
{
    public interface IMatchRequestWebService
    {
        Task<List<MatchRequestListViewModel>> GetRequestsByUserIdAsync(int userId);
        Task<bool> SendRequestAsync(int user1Id, int user2Id, string? message);
        Task<bool> AcceptRequestAsync(MatchRequestListViewModel request);
        Task<bool> RejectRequestAsync(MatchRequestListViewModel request);
        Task<bool> CancelRequestAsync(MatchRequestListViewModel request);
    }
}