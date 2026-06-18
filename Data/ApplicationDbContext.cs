using Microsoft.EntityFrameworkCore;
using TCA.API.Models;

namespace TCA.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Zone> Zones => Set<Zone>();
    public DbSet<Groupe> Groupes => Set<Groupe>();
    public DbSet<Camion> Camions => Set<Camion>();
    public DbSet<Chauffeur> Chauffeurs => Set<Chauffeur>();
    public DbSet<SuperviseurGroupe> SuperviseursGroupe => Set<SuperviseurGroupe>();
    public DbSet<SuperviseurZone> SuperviseursZone => Set<SuperviseurZone>();
    public DbSet<SuperviseurGeneral> SuperviseursGeneral => Set<SuperviseurGeneral>();
    public DbSet<Chargement> Chargements => Set<Chargement>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Zone>(entity =>
        {
            entity.HasIndex(z => z.Nom).IsUnique();
        });

        modelBuilder.Entity<Groupe>(entity =>
        {
            entity.HasIndex(g => g.Numero).IsUnique();
            entity.HasOne(g => g.Zone)
                .WithMany(z => z.Groupes)
                .HasForeignKey(g => g.ZoneId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Camion>(entity =>
        {
            entity.HasIndex(c => c.Numero).IsUnique();
            entity.HasOne(c => c.Groupe)
                .WithMany(g => g.Camions)
                .HasForeignKey(c => c.GroupeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Chauffeur>(entity =>
        {
            entity.HasOne(c => c.Camion)
                .WithOne(c => c.Chauffeur)
                .HasForeignKey<Chauffeur>(c => c.CamionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SuperviseurGroupe>(entity =>
        {
            entity.HasOne(s => s.Groupe)
                .WithMany(g => g.SuperviseursGroupe)
                .HasForeignKey(s => s.GroupeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SuperviseurZone>(entity =>
        {
            entity.HasOne(s => s.Zone)
                .WithMany(z => z.SuperviseursZone)
                .HasForeignKey(s => s.ZoneId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Chargement>(entity =>
        {
            entity.HasOne(c => c.Camion)
                .WithMany(c => c.Chargements)
                .HasForeignKey(c => c.CamionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Zone)
                .WithMany(z => z.Chargements)
                .HasForeignKey(c => c.ZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Chauffeur)
                .WithMany()
                .HasForeignKey(c => c.ChauffeurId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Username).IsUnique();

            entity.HasOne(u => u.Chauffeur)
                .WithMany()
                .HasForeignKey(u => u.ChauffeurId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.SuperviseurGroupe)
                .WithMany()
                .HasForeignKey(u => u.SuperviseurGroupeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.SuperviseurZone)
                .WithMany()
                .HasForeignKey(u => u.SuperviseurZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.SuperviseurGeneral)
                .WithMany()
                .HasForeignKey(u => u.SuperviseurGeneralId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
