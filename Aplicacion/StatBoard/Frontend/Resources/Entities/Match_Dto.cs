using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.Resources.Entities;

namespace Frontend.Resources.Entities
{
    public class Match_Dto
    {
        public System.Guid Id { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public string Place { get; set; }

        public string State { get; set; }

        public int MatchWeek { get; set; }

        public string Tournament { get; set; }

        public System.Guid IdTeamLocal { get; set; }

        public int GoalsTeamA { get; set; } = 0;

        public System.Guid IdTeamAway { get; set; }

        public int GoalsTeamB { get; set; } = 0;
    }
}
