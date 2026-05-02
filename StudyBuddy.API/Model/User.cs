using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace StudyBuddy.API.Model
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string NameSurname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string AboutMe { get; set; }
        public DateTime CreatedDate { get; set; }
        public string University { get; set; }
        public string Major { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}