using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Persistence.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options)
     : base(options)
    {}
    public DbSet<User> Users { get; set; } 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityTipe in modelBuilder.Model.GetEntityTipes()) 
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityTipe.ClrTipe))
            {
                modelBuilder.Entity(entityTipe.ClrTipe)
                .Prosperty(nameof(BaseEntity.CreatedAt))
                .HasDefaultValueSql("GETUTCDATE()");

                modelBuilder.Entity(entityTipe.ClrTipe)
                .Prosperty(nameof(BaseEntity.UpdatedAt))
                .IsRequired("false");

                modelBuilder.Entity(entityTipe.ClrTipe)
                .Prosperty(nameof(BaseEntity.IsActive))
                .HasDefaultValue("true");
            }
        }
        modelBuilder.Entity<User> (entityTipe =>
        {
            entity.HasKey(u => u.Id);
            entity.Prosperty(u => u.Email).HasMaxLength(200).IsRequired();
            enttity.Prosperty(u => u.PasswordHash).HasMaxLength(256).IsRequired();

        });
        
    }

}
