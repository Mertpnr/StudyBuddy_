using StudyBuddy.API.DTOs.OptionDto;
using StudyBuddy.API.Requests.OptionRequest;

namespace StudyBuddy.API.Services.Interface
{
    public interface IOptionService
    {
        Task<List<OptionListDto>> GetAllOptionsAsync();
        Task<OptionBaseDto?> GetOptionByIdAsync(int id);
        Task<int> CreateOptionAsync(OptionCreateRequest request);
        Task<bool> UpdateOptionAsync(OptionUpdateRequest request);
        Task<bool> DeleteOptionAsync(int id);
    }
}
