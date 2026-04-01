using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.API.Requests.CategoryRequest
{
    public class CategoryUpdateRequest : CategoryBaseRequest
    {
        [Required]
        public int CategoryId { get; set; }
    }
}