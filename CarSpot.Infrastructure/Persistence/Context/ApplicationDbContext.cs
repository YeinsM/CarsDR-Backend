using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;


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
    public required DbSet<Business> Business { get; set; }
    public required DbSet<Make> Makes { get; set; }
    public required DbSet<Model> Models { get; set; }
    public required DbSet<CabType> CabTypes { get; set; }
    public required DbSet<Condition> Conditions { get; set; }
    public required DbSet<Transmission> Transmissions { get; set; }
    public required DbSet<Drivetrain> Drivetrains { get; set; }
    public required DbSet<CylinderOption> CylinderOptions { get; set; }
    public required DbSet<MarketVersion> MarketVersions { get; set; }
    public required DbSet<Menu> Menus { get; set; }
    public required DbSet<EmailSettings> EmailSettings { get; set; }
    public required DbSet<Listing> Listings { get; set; } = null!;
    public DbSet<UserPlan> UserPlans { get; set; } = null!;
    public DbSet<Plan>? Plans { get; set; }
    public required DbSet<Color> Colors { get; set; }
    public DbSet<Comment>? Comments { get; set; }
    public DbSet<VehicleMediaFile> VehicleMediaFiles { get; set; } = null!;

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






















