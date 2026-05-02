using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddy.API.Model
{
    [Table("Matches")]
    public class Match
    {
        [Key]
        [Column("MatchId")]
        public int MatchId { get; set; }

        [Column("User1Id")]
        public int User1Id { get; set; }

        [Column("User2Id")]
        public int User2Id { get; set; }

        [Column("MatchPercent")]
        public decimal MatchPercent { get; set; } 

        [Column("MatchDate")]
        public DateTime MatchDate { get; set; }
    }
}