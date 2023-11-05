using CocktailApi.Persistance;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CocktailApi;

public class Program
{
    private static void Main(string[] args)
    {   
        var builder = WebApplication.CreateBuilder(args);
        {
            builder.Services.AddOptions();

            builder.Services.AddMvc();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = "https://dev-rglgi3ifuoda0lfd.eu.auth0.com/";
                options.Audience = "https://www.dirtydrinking.com";
            });

            builder.Services.AddDbContext<CocktailsDb>(opt => opt.UseInMemoryDatabase("CocktailsDb"));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services
              .AddAuthorization(options =>
              {
                  options.AddPolicy("delete:cocktail",
                  policy => policy.Requirements.Add(new HasPermissionRequirement("delete:cocktail"))
                  );
              })
              .AddCors(options =>
              {
                  options.AddPolicy(
                      name: "AllowLocalhost",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173");
                          policy.AllowAnyHeader();
                          policy.AllowCredentials();
                      });
              })
              .AddSingleton<IAuthorizationHandler, HasPermissionHandler>()
              .AddFastEndpoints()
              .SwaggerDocument();
        }

        var app = builder.Build();
        {
            app
                .UseAuthentication()
                .UseAuthorization()
                .UseCors("AllowLocalhost")
                .UseFastEndpoints();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerGen();
            }
        }

        app.Run();
    }
}