using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BdD_Android.Modelos
{
    public class Match : EntityBase
    {
        public DateTime Date { get; set; } = DateTime.Now;

        public string Place { get; set; }

        public string State { get; set; }

        public int MatchWeek { get; set; }

        public string Tournament { get; set; }

        [ForeignKey("TeamLocal")]
        public int IdTeamLocal { get; set; }

        public int GoalsTeamA { get; set; } = 0;

        [ForeignKey("TeamAway")]
        public int IdTeamAway { get; set; }

        public int GoalsTeamB { get; set; } = 0;
    }
}
