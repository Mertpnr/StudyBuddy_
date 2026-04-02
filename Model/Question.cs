using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddy.API.Model
{
    [Table("Questions")]
    public class Question
    {
        [Key]
        [Column("QuestionId")]
        public int QuestionId { get; set; }

        [Column("Question")]
        public string QuestionText { get; set; }

        [Column("CategoryId")]
        public int CategoryId { get; set; }

        [Column("MatchPercent")]
        public decimal MatchPercent { get; set; }
    }
}