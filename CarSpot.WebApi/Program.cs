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

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IEmailSettingsRepository, EmailSettingsRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IAuxiliarRepository<Transmission>, AuxiliarRepository<Transmission>>();
builder.Services.AddScoped<IAuxiliarRepository<Color>, AuxiliarRepository<Color>>();
builder.Services.AddScoped<IAuxiliarRepository<CabType>, AuxiliarRepository<CabType>>();
builder.Services.AddScoped<IAuxiliarRepository<Condition>, AuxiliarRepository<Condition>>();
builder.Services.AddScoped<IAuxiliarRepository<CylinderOption>, AuxiliarRepository<CylinderOption>>();
builder.Services.AddScoped<IAuxiliarRepository<Drivetrain>, AuxiliarRepository<Drivetrain>>();
builder.Services.AddScoped<IAuxiliarRepository<Make>, AuxiliarRepository<Make>>();
builder.Services.AddScoped<IAuxiliarRepository<MarketVersion>, AuxiliarRepository<MarketVersion>>();
builder.Services.AddScoped<IAuxiliarRepository<Model>, AuxiliarRepository<Model>>();
builder.Services.AddScoped<IAuxiliarRepository<Role>, AuxiliarRepository<Role>>();
builder.Services.AddScoped<IAuxiliarRepository<Version>, AuxiliarRepository<Version>>();


builder.Services.AddScoped<IAuxiliarRepository<Country>, AuxiliarRepository<Country>>();





















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
