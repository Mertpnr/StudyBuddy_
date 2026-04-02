using System.Collections.Generic;
using StudyBuddy.API.Model;

namespace StudyBuddy.API.Repository.Interface
{
    public interface IOptionRepository
    {
        Task<List<Option>> GetAllOptions();
        Task<Option> GetOptionById(int id);
        Task<int> InsertOption(Option option);
        Task<bool> UpdateOption(Option option);
        Task<bool> DeleteOption(int id);

    }
}