using StudyBuddy.WEB.Models.Quiz;
using StudyBuddy.WEB.Services.Interfaces;

namespace StudyBuddy.WEB.Services
{
    public class QuizWebService : IQuizWebService
    {
        private readonly IApiClientService _apiClientService;

        public QuizWebService(IApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public async Task<QuizPageViewModel> GetQuizAsync()
        {
            var questions = await _apiClientService.GetAsync<List<QuestionViewModel>>("Question/GetAll")
                            ?? new List<QuestionViewModel>();

            var options = await _apiClientService.GetAsync<List<OptionViewModel>>("Option/GetAll")
                          ?? new List<OptionViewModel>();

            foreach (var question in questions)
            {
                question.Options = options
                    .Where(x => x.QuestionId == question.QuestionId)
                    .OrderBy(x => x.OrderNo)
                    .ToList();
            }

            return new QuizPageViewModel
            {
                Questions = questions
            };
        }

        public async Task<bool> SubmitAnswersAsync(int userId, Dictionary<int, int> selectedOptions)
        {
            if (selectedOptions == null || selectedOptions.Count == 0)
                return false;

            foreach (var item in selectedOptions)
            {
                var request = new AnswerCreateViewModel
                {
                    UserId = userId,
                    QuestionId = item.Key,
                    OptionId = item.Value
                };

                var ok = await _apiClientService.PostAsync("Answers/Create", request);

                if (!ok)
                    return false;
            }

            return true;
        }
    }
}
