using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddy.API.Model
{
    [Table("Options")]
    public class Option
    {
        [Key]
        [Column("OptionId")]
        public int OptionId { get; set; }

        [Column("QuestionId")]
        public int QuestionId { get; set; }

        [Column("Text")]
        public string Text { get; set; }

        [Column("Vale")]
        public decimal Vale { get; set; }

        [Column("OrderNo")]
        public int OrderNo { get; set; }
    }
}