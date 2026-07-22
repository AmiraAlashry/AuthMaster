using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
namespace AuthMaster.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


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
