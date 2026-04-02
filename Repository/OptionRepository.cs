using System.Collections.Generic;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;

namespace StudyBuddy.API.Repository
{
    public class OptionRepository : IOptionRepository
    {
        private readonly IGenericRepository<Option> _genericRepository;

        public OptionRepository(IGenericRepository<Option> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<bool> DeleteOption(int id)
        {
            return await _genericRepository.Delete(id);
        }

        public async Task<List<Option>> GetAllOptions()
        {
            return await _genericRepository.GetAll();
        }

        public async Task<Option> GetOptionById(int id)
        {
            return await _genericRepository.GetById(id);
        }

        public async Task<int> InsertOption(Option option)
        {
            return await _genericRepository.Insert(option);
        }

        public async Task<bool> UpdateOption(Option option)
        {
            return await _genericRepository.Update(option);
        }
    }
}