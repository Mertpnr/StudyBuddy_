using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace StudyBuddy.API.Model
{
    [Table("Answers")]
    public class Answer
    {
        [Key]
        [Column("AnwerId")]
        public int AnswerId { get; set; }
        [Column("QuestionId")]
        public int QuestionId { get; set; }
        [Column("OptionId")]
        public int OptionId { get; set; }
        [Column("UserId")]
        public int UserId { get; set; }
    }
}