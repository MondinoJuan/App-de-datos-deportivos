using BdD_Android.Modelos;
using BdD_Android.Utilidades;
using Microsoft.EntityFrameworkCore;

namespace BdD_Android.DataAccess
{
    public class Match_DbContext : DbContext
    {
        public DbSet<Match> Matches { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conexionDB = $"Filename={ConexionDB.DevolverRuta("Match.db")}";
            optionsBuilder.UseSqlite(conexionDB);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>(entity =>
            {
                entity.HasKey(ma => ma.Id);
                entity.Property(ma => ma.Id).IsRequired().ValueGeneratedOnAdd();
            });
        }
    }
}
