using CarSpot.Application.Interfaces;
using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Application.Services;
using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;
using CarSpot.Domain.Events;
using CarSpot.Infrastructure.Persistence.Repositories;
using CarSpot.Infrastructure.Repositories;
using CarSpot.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarSpot.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
        {
            services.Configure<EmailSettings>(
            configuration.GetSection("EmailSettings"));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IVehicleMediaFileRepository, VehicleMediaFileRepository>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IEmailSettingsRepository, EmailSettingsRepository>();
            services.AddScoped<IBusinessRepository, BusinessRepository>();
            services.AddScoped<IListingRepository, ListingRepository>();
            services.AddScoped<IListingStatusRepository, ListingStatusRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IModelRepository, ModelRepository>();
            services.AddScoped<IMakeRepository, MakeRepository>();
            services.AddScoped<IAuxiliarRepository<Transmission>, AuxiliarRepository<Transmission>>();
            services.AddScoped<IAuxiliarRepository<VehicleType>, AuxiliarRepository<VehicleType>>();
            services.AddScoped<IAuxiliarRepository<Color>, AuxiliarRepository<Color>>();
            services.AddScoped<IAuxiliarRepository<CabType>, AuxiliarRepository<CabType>>();
            services.AddScoped<IAuxiliarRepository<Condition>, AuxiliarRepository<Condition>>();
            services.AddScoped<IAuxiliarRepository<CylinderOption>, AuxiliarRepository<CylinderOption>>();
            services.AddScoped<IAuxiliarRepository<Drivetrain>, AuxiliarRepository<Drivetrain>>();
            services.AddScoped<IAuxiliarRepository<MarketVersion>, AuxiliarRepository<MarketVersion>>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IAuxiliarRepository<VehicleVersion>, AuxiliarRepository<VehicleVersion>>();
            services.AddScoped<IAuxiliarRepository<Country>, AuxiliarRepository<Country>>();
            services.AddScoped<IAuxiliarRepository<City>, AuxiliarRepository<City>>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IPaginationService, PaginationService>();




            services.AddScoped<IDomainEventHandlerFactory, DomainEventHandlerFactory>();
            services.AddScoped<DomainEventsInterceptor>();

            services.AddScoped<IDomainEventHandler<UserRegisteredEvent>, UserRegisteredEventHandler>();
            services.AddScoped<IDomainEventHandler<VehicleCreatedEvent>, VehicleCreatedEventHandler>();

            return services;
        }
    }
}
