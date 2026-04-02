using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddy.API.DTOs
{
    public class UserCreateDto : UserBaseDto
    {
        public class User
        {
            public DateTime CreatedDate { get; set; }
        }
    }
}
