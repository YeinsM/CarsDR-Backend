using CarSpot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Persistence.Context;
    public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    { }
    
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email)
            .HasMaxLength(200)
            .IsRequired();
            entity.Property(u => u.PasswordHash)
            .HasMaxLength(256)
            .IsRequired();
            entity.Property(u => u.FullName)
            .HasMaxLength(100)
            .IsRequired();
            entity.Property(u => u.IsActive)
            .HasDefaultValue(true);
            entity.Property(u => u.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");
            entity.Property(u => u.UpdatedAt)
            .IsRequired(false);
        });
    }
}