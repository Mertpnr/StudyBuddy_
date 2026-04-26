using StudyBuddy.API.DTOs.MatchingDto;
using StudyBuddy.API.Enums;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Requests.MatchingRequest;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Services
{
	public class MatchingService : IMatchingService
	{
		private readonly IMatchRepository _matchRepository;

		public MatchingService(IMatchRepository matchRepository)
		{
			_matchRepository = matchRepository;
		}

		public async Task<MatchResultDto> CalculateMatchAsync(CalculateMatchRequest request)
		{
			var result = new MatchResultDto
			{
				SubjectMatchMode = request.SubjectMatchMode.ToString()
			};

			if (request.User1Id == request.User2Id)
			{
				result.Message = "A user cannot be matched with themselves.";
				return result;
			}

			int firstUserId = Math.Min(request.User1Id, request.User2Id);
			int secondUserId = Math.Max(request.User1Id, request.User2Id);

			result.User1Id = firstUserId;
			result.User2Id = secondUserId;

			bool subjectMatched = await _matchRepository.AreSubjectsMatchingAsync(
				firstUserId,
				secondUserId,
				request.SubjectQuestionId);

			result.SubjectMatched = subjectMatched;

			if (request.SubjectMatchMode == SubjectMatchMode.Required && !subjectMatched)
			{
				result.MatchPercent = 0m;
				result.SharedQuestionCount = 0;
				result.Message = "Subject matching is required, and the users have different subjects.";
				return result;
			}

			var comparableAnswers = await _matchRepository.GetComparableAnswersAsync(firstUserId, secondUserId);

			if (comparableAnswers == null || comparableAnswers.Count < request.MinimumSharedQuestions)
			{
				result.MatchPercent = 0m;
				result.SharedQuestionCount = comparableAnswers?.Count ?? 0;
				result.Message = "Not enough shared answered questions to calculate a reliable match.";
				return result;
			}

			decimal totalWeightedScore = 0m;
			decimal totalWeight = 0m;

			foreach (var item in comparableAnswers)
			{
				decimal similarity = 1m - Math.Abs(item.User1Value - item.User2Value);

				if (similarity < 0m) similarity = 0m;
				if (similarity > 1m) similarity = 1m;

				decimal weight = item.QuestionWeight;

				totalWeightedScore += similarity * weight;
				totalWeight += weight;
			}

			if (totalWeight <= 0m)
			{
				result.MatchPercent = 0m;
				result.SharedQuestionCount = comparableAnswers.Count;
				result.Message = "Total question weight is zero. Match could not be calculated.";
				return result;
			}

			decimal finalMatch = totalWeightedScore / totalWeight;

			if (request.SubjectMatchMode == SubjectMatchMode.Preferred && !subjectMatched)
			{
				finalMatch *= 0.85m;
			}

			finalMatch = Math.Round(finalMatch, 2);

			if (finalMatch < 0m) finalMatch = 0m;
			if (finalMatch > 1m) finalMatch = 1m;

			result.MatchPercent = finalMatch;
			result.SharedQuestionCount = comparableAnswers.Count;

			if (request.SaveResult)
			{
				var existing = await _matchRepository.GetByUsersAsync(firstUserId, secondUserId);

				if (existing == null)
				{
					var entity = new Match
					{
						User1Id = firstUserId,
						User2Id = secondUserId,
						MatchPercent = finalMatch,
						MatchDate = DateTime.UtcNow
					};

					await _matchRepository.Insert(entity);
				}
				else
				{
					existing.MatchPercent = finalMatch;
					existing.MatchDate = DateTime.UtcNow;

					await _matchRepository.Update(existing);
				}

				result.SavedToDatabase = true;
			}

			result.Message = "Match calculated successfully.";
			return result;
		}
	}
}
