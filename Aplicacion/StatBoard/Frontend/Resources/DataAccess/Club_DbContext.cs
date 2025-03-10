using Frontend.Resources.Modelos;
using BdD_Android.Utilidades;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Resources.DataAccess
{
    public class Club_DbContext : DbContext
    {
        public DbSet<Club> Clubes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conexionDB = $"Filename={ConexionDB.DevolverRuta("Club.db")}";
            optionsBuilder.UseSqlite(conexionDB);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Club>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
            });
        }
    }
}
