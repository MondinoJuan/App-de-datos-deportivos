using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Resources.Entities
{
    public class Match_Dto
    {
        public DateTime Date { get; set; } = DateTime.Now;

        public string Place { get; set; }

        public string State { get; set; }

        public int MatchWeek { get; set; }

        public string IdTournament { get; set; }

        public string IdTeamA { get; set; }

        public int GoalsTeamA { get; set; }

        public string IdTeamB { get; set; }

        public int GoalsTeamB { get; set; }
    }
}
