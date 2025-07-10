using System;
using System.IO;
using DotNetEnv;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using CarSpot.Infrastructure.Settings;

namespace CarSpot.WebApi.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        /// <summary>
        /// Carga variables de entorno desde .env y sobrescribe configuración sensible.
        /// </summary>
        public static WebApplicationBuilder AddEnvConfig(this WebApplicationBuilder builder)
        {
            // Cargar archivo .env (navega un nivel arriba para la raíz)
            var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
            if (File.Exists(envPath)) Env.Load(envPath);

            // Leer variables de entorno
            builder.Configuration.AddEnvironmentVariables();
            var cfg = builder.Configuration;

            // JwtSettings
            cfg["JwtSettings:Secret"] = Environment.GetEnvironmentVariable("JWT_SECRET") ?? cfg["JwtSettings:Secret"];
            cfg["JwtSettings:Issuer"] = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? cfg["JwtSettings:Issuer"];
            cfg["JwtSettings:Audience"] = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? cfg["JwtSettings:Audience"];
            cfg["JwtSettings:ExpiryMinutes"] = Environment.GetEnvironmentVariable("JWT_EXPIRY_MINUTES") ?? cfg["JwtSettings:ExpiryMinutes"];

            // EmailSettings
            cfg["EmailSettings:SmtpServer"] = Environment.GetEnvironmentVariable("EMAIL_SMTP_SERVER") ?? cfg["EmailSettings:SmtpServer"];
            cfg["EmailSettings:SmtpPort"] = Environment.GetEnvironmentVariable("EMAIL_SMTP_PORT") ?? cfg["EmailSettings:SmtpPort"];
            cfg["EmailSettings:FromEmail"] = Environment.GetEnvironmentVariable("EMAIL_FROM") ?? cfg["EmailSettings:FromEmail"];
            cfg["EmailSettings:FromPassword"] = Environment.GetEnvironmentVariable("EMAIL_PASSWORD") ?? cfg["EmailSettings:FromPassword"];

            // CloudinarySettings
            cfg["CloudinarySettings:CloudName"] = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME") ?? cfg["CloudinarySettings:CloudName"];
            cfg["CloudinarySettings:ApiKey"] = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY") ?? cfg["CloudinarySettings:ApiKey"];
            cfg["CloudinarySettings:ApiSecret"] = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET") ?? cfg["CloudinarySettings:ApiSecret"];

            // ConnectionStrings:Default
            var server = Environment.GetEnvironmentVariable("DB_SERVER");
            var port = Environment.GetEnvironmentVariable("DB_PORT");
            var name = Environment.GetEnvironmentVariable("DB_NAME");
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var pwd = Environment.GetEnvironmentVariable("DB_PASSWORD");
            if (!string.IsNullOrEmpty(server)
                && !string.IsNullOrEmpty(port)
                && !string.IsNullOrEmpty(name)
                && !string.IsNullOrEmpty(user)
                && !string.IsNullOrEmpty(pwd))
            {
                cfg["ConnectionStrings:Default"] =
                    $"Server=tcp:{server},{port};Initial Catalog={name};Persist Security Info=False;User ID={user};Password={pwd};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            }

            return builder;
        }
    }
}
