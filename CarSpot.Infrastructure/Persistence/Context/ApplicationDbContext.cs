using CarSpot.Domain;
using CarSpot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarSpot.Infrastructure.Persistence.Context;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    { }

    public DbSet<User> Users { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Make> Makes { get; set; }
    public DbSet<Model> Models { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(e => e.FirstName)
            .HasMaxLength(100)
            .IsRequired();
            entity.Property(e => e.LastName)
            .HasMaxLength(100)
            .IsRequired();
            entity.Property(u => u.Email)
            .HasMaxLength(200)
            .IsRequired();
            entity.Property(u => u.Password)
            .HasMaxLength(256)
            .IsRequired();
            entity.Property(u => u.Username)
            .HasMaxLength(100)
            .IsRequired();
            entity.Property(u => u.IsActive)
            .HasDefaultValue(true);
            entity.Property(u => u.CreatedAt)
            .HasDefaultValueSql("GETDATE()");
            entity.Property(u => u.UpdatedAt)
            .IsRequired(false);
        });

        modelBuilder.Entity<Make>(entity =>
         {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Name)
             .HasMaxLength(100)
             .IsRequired();
        });

        modelBuilder.Entity<Model>(entity =>
        {
       entity.HasKey(m => m.Id);
       entity.Property(m => m.Name)
             .HasMaxLength(100)
             .IsRequired();
       entity.HasOne(m => m.Make)
             .WithMany(make => make.Models)
             .HasForeignKey(m => m.MakeId);
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(v => v.Id);
            entity.Property(v => v.VIN)
                  .HasMaxLength(50)
                  .IsRequired();
            entity.Property(v => v.Year)
                  .IsRequired();
            entity.Property(v => v.Color)
                  .HasMaxLength(50)
                  .IsRequired();
            entity.HasOne(v => v.Model)
                  .WithMany(m => m.Vehicles)
                  .HasForeignKey(v => v.ModelId);
        });

    }
}