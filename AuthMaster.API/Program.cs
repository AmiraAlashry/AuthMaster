using AuthMaster.Application.DTOs.Auth;
using AuthMaster.Application.Helpers;
using AuthMaster.Application.Interfaces.AuthServices;
using AuthMaster.Application.Interfaces.CommonServices;
using AuthMaster.Application.Services.AuthServices;
using AuthMaster.Domain.Entities;
using AuthMaster.Domain.Interfaces.AuthRepositories;
using AuthMaster.Infrastructure.Data;
using AuthMaster.Infrastructure.Repositories.AuthRepositories;
using AuthMaster.Infrastructure.Services.CommonServices;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
namespace AuthMaster.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddValidatorsFromAssemblyContaining<ValidationFormat>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "AuthMaster API",
                    Version = "v1",
                    Description = "API for Authentication and Authorization Management"
                });

                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter your valid token"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                    }
                },
                Array.Empty<string>()
                    }
                });
            });
            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthMaster API v1");

            });
            app.UseHttpsRedirection();
            app.UseAuthentication(); 
            app.UseAuthorization();
            app.MapControllers();
            app.Run();




        }
    }
}
