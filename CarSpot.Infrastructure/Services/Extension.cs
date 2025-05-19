using CarSpot.Application. Interfaces; using CarSpot.Application.Services; using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence.Repositories; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions. DependencyInjection;

namespace CarSpot. Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices (this IServiceCollection services, 
        IConfiguration configuration)
    ();
    ();
    {
        services.Configure<EmailSettings>(
        configuration.GetSection("EmailSettings"));
        services.AddScoped<IUserRepository, UserRepository>(); services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<I EmailService, EmailService>();
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<I EmailSettings Repository, EmailSettings Repository>();
        services.AddScoped<IListingRepository, ListingRepository>();
        services.AddScoped<IListingStatus Repository, ListingStatus Repository>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IAuxiliarRepository<Transmission>, AuxiliarRepository<Transmission>>();
        services.AddScoped<IAuxiliarRepository<Color>, AuxiliarRepository<Color>>();
        services.AddScoped<IAuxiliarRepository<CabType>, AuxiliarRepository<CabType>>();
        services.AddScoped<IAuxiliarRepository<Condition>, AuxiliarRepository<Condition>>(); services.AddScoped<IAuxiliarRepository<CylinderOption>, AuxiliarRepository<CylinderOption>>
        services.AddScoped<IAuxiliarRepository<Drivetrain>, AuxiliarRepository<Drivetrain>>(); services.AddScoped<IAuxiliarRepository<Make>, AuxiliarRepository<Make>>();
        services.AddScoped<IAuxiliarRepository<MarketVersion>, AuxiliarRepository<MarketVersion>>(); services.AddScoped<IAuxiliarRepository<Model>, AuxiliarRepository<Model>>();
        services.AddScoped<IAuxiliarRepository<Role>, AuxiliarRepository<role>>();
        services.AddScoped<IAuxiliarRepository<VehicleVersion>, AuxiliarRepository<VehicleVersion>>
        services.AddScoped<IAuxiliarRepository<Country>, AuxiliarRepository<Country>>();
    }
}
return services;