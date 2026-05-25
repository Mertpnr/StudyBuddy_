using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddy.API.Model
{
    [Table("Chat")]
    public class Chat
    {
        [Key]
        public int ChatId { get; set; }

        public int MatchRequestId { get; set; }

        public int UserId { get; set; }

        public string? Message { get; set; }

        public DateTime Date { get; set; }
    }
}