using System.ComponentModel.DataAnnotations;

namespace Entities.Entities
{
    public class Match : EntityBase
    {
        [Required]
        public int PlayTime { get; set; } = 60;      // Seguro lo termine sacando

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Place { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public int MatchWeek {get; set;}
        
        [Required]
        public string IdTournament { get; set; }

        [Required]
        public string IdTeamA {  get; set; }

        [Required]
        public int GoalsTeamA { get; set; }

        [Required]
        public string IdTeamB { get; set; }

        [Required]
        public int GoalsTeamB { get; set; }
    }
}
