using Frontend.Resources.Modelos;
using BdD_Android.Utilidades;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Resources.DataAccess;

public class StatBoard_DbContext : DbContext
{
    public StatBoard_DbContext()
    {
        this.Database.EnsureCreated();
    }

    public DbSet<Club> Clubes { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<PlayerAction> PlayerActions { get; set; }
    public DbSet<PlayerMatch> PlayerMatches { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string conexionDB = $"Filename={ConexionDB.DevolverRuta("AppDatabase.db")}";
        optionsBuilder.UseSqlite(conexionDB);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<PlayerAction>(entity =>
        {
            entity.HasKey(pa => pa.Id);
            entity.Property(pa => pa.Id).IsRequired().ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<PlayerMatch>(entity =>
        {
            entity.HasKey(pm => pm.Id);
            entity.Property(pm => pm.Id).IsRequired().ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
        });
    }
}
