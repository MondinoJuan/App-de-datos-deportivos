using Frontend.Resources.Modelos;
using BdD_Android.Utilidades;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Resources.DataAccess
{
    public class PlayerMatch_DbContext : DbContext
    {
        public DbSet<PlayerMatch> PlayerMatchs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conexionDB = $"Filename={ConexionDB.DevolverRuta("PlayerMatch.db")}";
            optionsBuilder.UseSqlite(conexionDB);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerMatch>(entity =>
            {
                entity.HasKey(pm => pm.Id);
                entity.Property(pm => pm.Id).IsRequired().ValueGeneratedOnAdd();
            });
        }
    }
}
