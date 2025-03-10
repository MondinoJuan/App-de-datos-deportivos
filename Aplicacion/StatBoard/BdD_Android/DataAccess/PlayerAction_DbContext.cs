using BdD_Android.Modelos;
using BdD_Android.Utilidades;
using Microsoft.EntityFrameworkCore;

namespace BdD_Android.DataAccess
{
    public class PlayerAction_DbContext : DbContext
    {
        public DbSet<PlayerAction> PlayerActions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conexionDB = $"Filename={ConexionDB.DevolverRuta("PlayerAction.db")}";
            optionsBuilder.UseSqlite(conexionDB);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerAction>(entity =>
            {
                entity.HasKey(pa => pa.Id);
                entity.Property(pa => pa.Id).IsRequired().ValueGeneratedOnAdd();
            });
        }
    }
}
