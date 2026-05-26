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

            ApplyQuestionTextFallbacks(questions);

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

        private static void ApplyQuestionTextFallbacks(List<QuestionViewModel> questions)
        {
            string[] defaultQuestions =
            {
                "What subject do you want to study most often?",
                "What is your preferred study style?",
                "When do you usually prefer to study?",
                "How often do you want to meet with a study buddy?",
                "What is your main goal when studying with someone?",
                "How do you prefer to communicate while studying?",
                "How much structure do you like in a study session?",
                "What is your current confidence level in the subject you want to study?",
                "How do you handle difficult topics?",
                "What kind of study buddy are you looking for?"
            };

            for (int i = 0; i < questions.Count && i < defaultQuestions.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(questions[i].Question))
                {
                    questions[i].Question = defaultQuestions[i];
                }
            }
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