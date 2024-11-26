using System;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class Context : DbContext
    {
        public Context()
        {
            this.Database.EnsureCreated();
        }

        public DbSet<PlayerAction> PlayersActions { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Match> Matchs { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerMatch> PlayersMatchs { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"aaaaa");
        }
    }
}
