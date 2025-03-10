using BdD_Android.Modelos;
using BdD_Android.Utilidades;
using Microsoft.EntityFrameworkCore;

namespace BdD_Android.DataAccess
{
    public class Player_DbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conexionDB = $"Filename={ConexionDB.DevolverRuta("Player.db")}";
            optionsBuilder.UseSqlite(conexionDB);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(pl => pl.Id);
                entity.Property(pl => pl.Id).IsRequired().ValueGeneratedOnAdd();
            });
        }
    }
}
