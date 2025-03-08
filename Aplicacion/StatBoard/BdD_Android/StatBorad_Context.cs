using System;
using Microsoft.EntityFrameworkCore;
using BdD_Android.Modelos;

namespace BdD_Android
{
    public class StatBorad_Context : DbContext
    {
        public StatBorad_Context()
        {
            this.Database.EnsureCreated();
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerMatch> PlayerMatches { get; set; }
        public DbSet<PlayerAction> PlayerActions { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
    }
}
