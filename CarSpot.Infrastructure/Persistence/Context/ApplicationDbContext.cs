using CarSpot.Domain;
using CarSpot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using CarSpot.Domain.ValueObjects;
using System.Text.Json;
using System.ComponentModel.DataAnnotations.Schema;


namespace CarSpot.Infrastructure.Persistence.Context;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {

    }


    public required DbSet<User> Users { get; set; } = null!;
    public required DbSet<Vehicle> Vehicles { get; set; }
    public required DbSet<Make> Makes { get; set; }
    public required DbSet<Model> Models { get; set; }
    public required DbSet<Menu> Menus { get; set; }
    public required DbSet<EmailSettings> EmailSettings { get; set; }
    public DbSet<Listing> Listings { get; set; } = null!;
    public required DbSet<Color> Colors { get; set; }
    public DbSet<Comment>? Comments { get; set; }
    public DbSet<VehicleImage> VehicleImages { get; set; } = null!;

    public DbSet<Country>? Countries { get; set; }
    public DbSet<Currency> Currencies { get; set; } = null!;

    public DbSet<ListingStatus> ListingStatuses { get; set; } = null!;
    public DbSet<VehicleVersion> VehicleVersions { get; set; } = null!;
    public DbSet<Role>? Roles { get; set; }















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
            var passwordConverter = new ValueConverter<HashedPassword, string>(
            v => v.Value,
            v => HashedPassword.FromHashed(v)
            );
            entity.Property(u => u.Password)
                .HasConversion(passwordConverter)
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
                  .IsRequired()
                  .HasMaxLength(50);
            entity.Property(v => v.Year)
                  .IsRequired();
            entity.HasOne(v => v.Model)
                  .WithMany(m => m.Vehicles)
                  .HasForeignKey(v => v.ModelId);



        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Label).IsRequired();
            entity.Property(m => m.Icon).IsRequired();
            entity.Property(m => m.To).IsRequired(false);
            entity.Property(m => m.ParentId).IsRequired(false);


            entity.HasMany(m => m.Children)
               .WithOne()
               .HasForeignKey(m => m.ParentId)
               .OnDelete(DeleteBehavior.Restrict);
        });


        modelBuilder.Entity<EmailSettings>(entity =>
         {
             entity.ToTable("EmailSettings");

             entity.Property(e => e.SmtpServer).IsRequired().HasMaxLength(100);
             entity.Property(e => e.SmtpPort).IsRequired();
             entity.Property(e => e.FromEmail).IsRequired().HasMaxLength(100);
             entity.Property(e => e.FromPassword).IsRequired();
         });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name)
                .HasMaxLength(50)
                .IsRequired();
        });

        modelBuilder.Entity<Comment>(entity =>
           {
               entity.HasKey(c => c.Id);

               entity.Property(c => c.Content)
                   .IsRequired()
                   .HasMaxLength(1000);

               entity.Property(c => c.CreatedAt)
                   .IsRequired();

               entity.HasOne(c => c.User)
                   .WithMany(u => u.Comments)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

               entity.HasOne(c => c.Vehicle)
                   .WithMany(v => v.Comments)
                   .HasForeignKey(c => c.VehicleId)
                   .OnDelete(DeleteBehavior.Cascade);
           });

        modelBuilder.Entity<VehicleImage>(entity =>
        {
            entity.HasKey(v => v.Id);

            entity.Property(v => v.ImageUrl)
            .IsRequired()
            .HasMaxLength(1000);

            entity.HasOne(v => v.Vehicle)
            .WithMany(v => v.Images)
            .HasForeignKey(v => v.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Listing>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            entity.HasOne(l => l.Currency)
            .WithMany()
            .HasForeignKey(l => l.CurrencyId);

            entity.HasMany(p => p.Images)
                .WithOne(img => img.Listing)
                .HasForeignKey(img => img.ListingId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ListingStatus>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Symbol).IsRequired().HasMaxLength(10);
        });

        modelBuilder.Entity<VehicleVersion>(entity =>
        {
            entity.ToTable("Versions");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name)
                .HasMaxLength(50)
                .IsRequired();
        });



    }
}



