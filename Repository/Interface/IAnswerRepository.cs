using System.Collections.Generic;
using StudyBuddy.API.Model;

namespace StudyBuddy.API.Repository.Interface
{
    public interface IAnswerRepository
    {
        Task<List<Answer>> GetAllAswers();
        Task<Answer> GetAnswerId(int answerId);
        Task<int> InsertAnswer(Answer answer);
        Task<bool> UpdateAnswer(Answer answer);
        Task<bool> DeleteAnswer(int answerId);
    }
}