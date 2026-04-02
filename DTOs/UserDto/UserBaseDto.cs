using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddy.API.DTOs
{
    public class UserBaseDto
    {
        [Table("Users")]
        public class User
        {
            public string NameSurname { get; set; }
            public string Email { get; set; }
            public string AboutMe { get; set; }
            public string University { get; set; }
            public string Major { get; set; }
        }
    }
}
