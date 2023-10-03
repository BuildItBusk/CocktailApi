using CocktailApi;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
{
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
            policy  =>
            {
                policy.WithOrigins("http://localhost:5173");
                policy.AllowAnyHeader();
                policy.AllowCredentials();
            });
    })
    .AddSingleton<IAuthorizationHandler, HasPermissionHandler>()
    .AddFastEndpoints()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();
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
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}

app.Run();