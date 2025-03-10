using BdD_Android.Modelos;
using BdD_Android.Utilidades;
using Microsoft.EntityFrameworkCore;

namespace BdD_Android.DataAccess
{
    public class PlayerMatch_DbContext : DbContext
    {
        public DbSet<PlayerMatch> PlayerMatches { get; set; }
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
