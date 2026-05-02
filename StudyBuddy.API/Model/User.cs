using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddy.API.Model
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("UserId")]
        public int UserId { get; set; }

        [Column("UserGuid")]
        public Guid UserGuid { get; set; }

        [Column("NameSurname")]
        public string NameSurname { get; set; } = string.Empty;

        [Column("Email")]
        public string Email { get; set; } = string.Empty;

        [Column("PasswordHash")]
        public string? PasswordHash { get; set; }

        [Column("AboutMe")]
        public string? AboutMe { get; set; }

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Column("University")]
        public string University { get; set; } = string.Empty;

        [Column("Major")]
        public string? Major { get; set; }

        [Column("UpdatedDate")]
        public DateTime? UpdatedDate { get; set; }
    }
}