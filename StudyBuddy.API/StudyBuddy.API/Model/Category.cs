using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
namespace StudyBuddy.API.Model

{
    [Table("Category")]
    public class Category
    {
        [Key]
        [Column("CategoryId")]
        public int CategoryId { get; set; }
        [Column("CategoryName")]
        public string CategoryName { get; set; }
    }

}
