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
                $"MatchRequests/user/{userId}"
            ) ?? new List<MatchRequestListViewModel>();

            foreach (var request in requests)
            {
                var sender = await _userWebService.GetUserByIdAsync(request.SenderUserId);
                var receiver = await _userWebService.GetUserByIdAsync(request.ReceiverUserId);

                request.SenderName = sender?.NameSurname;
                request.ReceiverName = receiver?.NameSurname;
            }

            return requests;
        }

        public async Task<bool> SendRequestAsync(int senderUserId, int receiverUserId)
        {
            var request = new
            {
                senderUserId = senderUserId,
                receiverUserId = receiverUserId,
                status = 0
            };

            return await _apiClientService.PostAsync("MatchRequests", request);
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
            var updateRequest = new
            {
                matchRequestId = request.MatchRequestId,
                senderUserId = request.SenderUserId,
                receiverUserId = request.ReceiverUserId,
                status = status
            };

            return await _apiClientService.PutAsync("MatchRequests", updateRequest);
        }
    }
}
