using CarSpot.Infrastructure.Extensions;
using CarSpot.Infrastructure.Middleware;
using CarSpot.Infrastructure.Persistence.Context;
using CarSpot.Infrastructure.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CarSpot.WebApi.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Cargar configuración sensible desde .env y variables de entorno
builder.AddEnvConfig();

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));


builder.Services.AddScoped<IPhotoService, PhotoService>();


// Register application services
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSwaggerWithJwt();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


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

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseCors("AllowWebApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
