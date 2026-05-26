using StudyBuddy.API.DTOs.MatchRequestDto;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Requests.MatchRequestRequest;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Services
{
    public class MatchRequestService : IMatchRequestService
    {
        private readonly IMatchRequestRepository _repository;

        public MatchRequestService(IMatchRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MatchRequestListDto>> GetAllMatchRequestsAsync()
        {
            var list = await _repository.GetAll();
            return MapList(list);
        }

        public async Task<List<MatchRequestListDto>> GetMatchRequestsByUserIdAsync(int userId)
        {
            var list = await _repository.GetByUserIdAsync(userId);
            return MapList(list);
        }

        public async Task<MatchRequestBaseDto?> GetMatchRequestByIdAsync(int id)
        {
            var entity = await _repository.GetById(id);
            if (entity == null) return null;

            return new MatchRequestBaseDto
            {
                User1Id = entity.User1Id,
                User2Id = entity.User2Id,
                Status = entity.Status,
                Message = entity.Message,
                CreatedDate = entity.CreatedDate
            };
        }

        public async Task<int?> CreateMatchRequestAsync(MatchRequestCreateRequest request)
        {
            if (request.User1Id <= 0 || request.User2Id <= 0 || request.User1Id == request.User2Id)
                return null;

            var activeRequest = await _repository.GetActiveBetweenUsersAsync(
                request.User1Id,
                request.User2Id);

            if (activeRequest != null)
                return null;

            var entity = new MatchRequest
            {
                User1Id = request.User1Id,
                User2Id = request.User2Id,
                Status = request.Status,
                Message = string.IsNullOrWhiteSpace(request.Message)
                    ? "This user wants to match with you."
                    : request.Message,
                CreatedDate = request.CreatedDate == default
                    ? DateTime.UtcNow
                    : request.CreatedDate
            };

            return await _repository.InsertReturnId(entity);
        }

        public async Task<bool> UpdateMatchRequestAsync(MatchRequestUpdateRequest request)
        {
            var existing = await _repository.GetById(request.MatchRequestId);
            if (existing == null) return false;

            existing.User1Id = request.User1Id;
            existing.User2Id = request.User2Id;
            existing.Status = request.Status;
            existing.Message = request.Message ?? existing.Message;

            if (request.CreatedDate != default)
                existing.CreatedDate = request.CreatedDate;

            return await _repository.Update(existing);
        }

        public async Task<bool> DeleteMatchRequestAsync(int id)
        {
            var existing = await _repository.GetById(id);
            if (existing == null) return false;

            return await _repository.Delete(id);
        }

        private static List<MatchRequestListDto> MapList(IEnumerable<MatchRequest> list)
        {
            return list.Select(x => new MatchRequestListDto
            {
                MatchRequestId = x.MatchRequestId,
                User1Id = x.User1Id,
                User2Id = x.User2Id,
                Status = x.Status,
                Message = x.Message,
                CreatedDate = x.CreatedDate
            }).ToList();
        }
    }
}