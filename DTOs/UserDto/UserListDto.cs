using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddy.API.DTOs
{
    public class UserListDto : UserBaseDto
    {
        [Key]
        public int UserId { get; set; }
    }
}
