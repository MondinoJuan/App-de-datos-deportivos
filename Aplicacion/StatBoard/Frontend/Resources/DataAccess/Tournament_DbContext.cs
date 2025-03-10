using Frontend.Resources.Modelos;
using BdD_Android.Utilidades;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Resources.DataAccess
{
    public class Tournament_DbContext : DbContext
    {
        public DbSet<Tournament> Tournaments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conexionDB = $"Filename={ConexionDB.DevolverRuta("Tournament.db")}";
            optionsBuilder.UseSqlite(conexionDB);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tournament>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
            });
        }
    }
}
