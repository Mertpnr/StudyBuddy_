using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddy.API.Model
{
    [Table("MatchRequests")]
    public class MatchRequest
    {
        [Key]
        [Column("MatchRequestId")]
        public int MatchRequestId { get; set; }

        [Column("User1Id")]
        public int User1Id { get; set; }

        [Column("User2Id")]
        public int User2Id { get; set; }

        [Column("Status")]
        public byte Status { get; set; }

        [Column("Message")]
        public string Message { get; set; } 

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }
    }
}