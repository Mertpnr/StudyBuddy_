using StudyBuddy.API.DTOs.ChatDto;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Requests.ChatRequest;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _repository;

        public ChatService(IChatRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ChatListDto>> GetMessagesAsync(int matchRequestId, int userId)
        {
            var canAccess = await _repository.UserCanAccessChatAsync(matchRequestId, userId);

            if (!canAccess)
                return new List<ChatListDto>();

            var messages = await _repository.GetByMatchRequestIdAsync(matchRequestId);

            return messages.Select(x => new ChatListDto
            {
                ChatId = x.ChatId,
                MatchRequestId = x.MatchRequestId,
                UserId = x.UserId,
                Message = x.Message,
                Date = x.Date
            }).ToList();
        }

        public async Task<int?> CreateMessageAsync(ChatCreateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
                return null;

            var canAccess = await _repository.UserCanAccessChatAsync(
                request.MatchRequestId,
                request.UserId
            );

            if (!canAccess)
                return null;

            var entity = new Chat
            {
                MatchRequestId = request.MatchRequestId,
                UserId = request.UserId,
                Message = request.Message.Trim(),
                Date = DateTime.UtcNow
            };

            return await _repository.AddMessageAsync(entity);
        }

        public async Task<bool> UpdateMessageAsync(ChatUpdateRequest request)
        {
            if (request.ChatId <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(request.Message))
                return false;

            var canAccess = await _repository.UserCanAccessChatAsync(
                request.MatchRequestId,
                request.UserId
            );

            if (!canAccess)
                return false;

            var entity = new Chat
            {
                ChatId = request.ChatId,
                MatchRequestId = request.MatchRequestId,
                UserId = request.UserId,
                Message = request.Message.Trim()
            };

            return await _repository.UpdateMessageAsync(entity);
        }

        public async Task<bool> DeleteMessageAsync(int chatId)
        {
            var existing = await _repository.GetById(chatId);

            if (existing == null)
                return false;

            return await _repository.Delete(chatId);
        }
    }
}