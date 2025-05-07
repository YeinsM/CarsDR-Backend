using Microsoft.EntityFrameworkCore;
using CarSpot.Infrastructure.Persistence.Context;
using CarSpot.Infrastructure.Persistence.Repositories;
using CarSpot.Application.Interfaces;
using CarSpot.Application.Services;
using CarSpot.Domain.Entities;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGenericRepository<Vehicle>, VehicleRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IEmailSettingsRepository, EmailSettingsRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
services.AddScoped<IAuxiliarRepository<Transmission>, AuxiliarRepository<Transmission>>();
services.AddScoped<IAuxiliarRepository<Color>, AuxiliarRepository<Color>>();
services.AddScoped<IAuxiliarRepository<CabType>, AuxiliarRepository<CabType>>();
services.AddScoped<IAuxiliarRepository<Condition>, AuxiliarRepository<Condition>>();
services.AddScoped<IAuxiliarRepository<CylinderOption>, AuxiliarRepository<CylinderOption>>();
services.AddScoped<IAuxiliarRepository<Drivetrain>, AuxiliarRepository<Drivetrain>>();
services.AddScoped<IAuxiliarRepository<Make>, AuxiliarRepository<Make>>();
services.AddScoped<IAuxiliarRepository<MarketVersion>, AuxiliarRepository<MarketVersion>>();
services.AddScoped<IAuxiliarRepository<Model>, AuxiliarRepository<Model>>();
services.AddScoped<IAuxiliarRepository<Role>, AuxiliarRepository<Role>>();
services.AddScoped<IAuxiliarRepository<Version>, AuxiliarRepository<Version>>();
services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IVehicleImageRepository, VehicleImageRepository>();
builder.Services.AddScoped<IPublicationRepository, PublicationRepository>();




















builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<IEmailService, EmailService>();




builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Security policy allowing connections from any source to the API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp",
        policy =>
        {
            policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseCors("AllowWebApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
