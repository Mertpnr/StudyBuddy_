using StudyBuddy.WEB.Models.MatchRequest;
using StudyBuddy.WEB.Services.Interfaces;

namespace StudyBuddy.WEB.Services
{
    public class MatchRequestWebService : IMatchRequestWebService
    {
        private readonly IApiClientService _apiClientService;
        private readonly IUserWebService _userWebService;

        public MatchRequestWebService(
            IApiClientService apiClientService,
            IUserWebService userWebService)
        {
            _apiClientService = apiClientService;
            _userWebService = userWebService;
        }

        public async Task<List<MatchRequestListViewModel>> GetRequestsByUserIdAsync(int userId)
        {
            var requests = await _apiClientService.GetAsync<List<MatchRequestListViewModel>>(
                $"MatchRequest/GetByUser/{userId}"
            ) ?? new List<MatchRequestListViewModel>();

            foreach (var request in requests)
            {
                var user1 = await _userWebService.GetUserByIdAsync(request.User1Id);
                var user2 = await _userWebService.GetUserByIdAsync(request.User2Id);

                request.User1Name = user1?.NameSurname;
                request.User2Name = user2?.NameSurname;
            }

            return requests;
        }

        public async Task<bool> SendRequestAsync(int user1Id, int user2Id, string? message)
        {
            var user1 = await _userWebService.GetUserByIdAsync(user1Id);
            var user1Name = string.IsNullOrWhiteSpace(user1?.NameSurname)
                ? "A user"
                : user1.NameSurname;

            var request = new
            {
                user1Id,
                user2Id,
                status = 0,
                message = string.IsNullOrWhiteSpace(message)
                    ? $"{user1Name} wants to match with you."
                    : message,
                createdDate = DateTime.UtcNow
            };

            return await _apiClientService.PostAsync("MatchRequest/Create", request);
        }

        public async Task<bool> AcceptRequestAsync(MatchRequestListViewModel request)
        {
            return await UpdateStatusAsync(request, 1);
        }

        public async Task<bool> RejectRequestAsync(MatchRequestListViewModel request)
        {
            return await UpdateStatusAsync(request, 2);
        }

        public async Task<bool> CancelRequestAsync(MatchRequestListViewModel request)
        {
            return await UpdateStatusAsync(request, 3);
        }

        private async Task<bool> UpdateStatusAsync(MatchRequestListViewModel request, int status)
        {
            var message = status switch
            {
                1 => $"{request.User2Name ?? "The user"} accepted your match request.",
                2 => $"{request.User2Name ?? "The user"} declined your match request.",
                3 => "Match request was cancelled.",
                _ => request.Message
            };

            var updateRequest = new
            {
                matchRequestId = request.MatchRequestId,
                user1Id = request.User1Id,
                user2Id = request.User2Id,
                status,
                message,
                createdDate = request.CreatedDate
            };

            return await _apiClientService.PutAsync("MatchRequest/Update", updateRequest);
        }
    }
}