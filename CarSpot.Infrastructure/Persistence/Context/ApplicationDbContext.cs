using CarSpot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CarSpot.Infrastructure.Persistence.Configurations;


namespace CarSpot.Infrastructure.Persistence.Context;
public class ApplicationDbContext : DbContext
{
    private readonly DomainEventsInterceptor _domainEventsInterceptor;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, DomainEventsInterceptor domainEventsInterceptor)
    : base(options)
    {
        _domainEventsInterceptor = domainEventsInterceptor;

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_domainEventsInterceptor);
        base.OnConfiguring(optionsBuilder);
    }


    public required DbSet<User> Users { get; set; } = null!;
    public required DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Business>? Business { get; set; }
    public required DbSet<Make> Makes { get; set; }
    public required DbSet<Model> Models { get; set; }
    public required DbSet<Menu> Menus { get; set; }
    public required DbSet<EmailSettings> EmailSettings { get; set; }
    public DbSet<Listing> Listings { get; set; } = null!;
    public required DbSet<Color> Colors { get; set; }
    public DbSet<Comment>? Comments { get; set; }
    public DbSet<VehicleImage> VehicleImages { get; set; } = null!;

    public DbSet<Country>? Countries { get; set; }
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<Currency> Currencies { get; set; } = null!;

    public DbSet<ListingStatus> ListingStatuses { get; set; } = null!;
    public DbSet<VehicleVersion> VehicleVersions { get; set; } = null!;
    public DbSet<Role>? Roles { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ModelConfiguration.Configure(modelBuilder);
    }
}






















