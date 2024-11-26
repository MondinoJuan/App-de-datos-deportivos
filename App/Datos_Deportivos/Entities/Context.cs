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
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-6HBL1R6\SQLEXPRESS;Initial Catalog=Datos_Deportivos;Integrated Security=True;Encrypt=False");
        }
    }
}
