using CocktailApi.Persistance;
using CocktailApi.Persistance.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbDescriber = services.SingleOrDefault(
                d => d.ServiceType == typeof(CocktailsDb));

            services.Remove(dbDescriber!);
            services.AddDbContext<CocktailsDb>(opt => opt.UseInMemoryDatabase("CocktailsDb"));
            SeedDatabase(services);
        });

        builder.UseEnvironment("Development");
    }

    private static IServiceCollection SeedDatabase(IServiceCollection services)
    {
        
        var db = services.BuildServiceProvider().GetService<CocktailsDb>();

        var cocktails = new List<Cocktail> 
        {
            new()
            {
                Id = new Guid("4b54e698-706e-4fd9-a9e4-94407c2ed735"),
                Name = "Margarita",
                Description = "A classic and refreshing cocktail.",
                ImageUrl = new Uri("https://www.example.com/margarita.jpg"),
                Ingredients =
                [
                    new()
                    {
                        Name = "Tequila",
                        Quantity = 2,
                        Unit = "oz"
                    },
                    new()
                    {
                        Name = "Triple Sec",
                        Quantity = 1,
                        Unit = "oz"
                    },
                    new()
                    {
                        Name = "Lime Juice",
                        Quantity = 1,
                        Unit = "oz"
                    }
                ]
            },
            new() 
            {
                Id = Guid.NewGuid(),
                Name = "Old Fashioned",
                Description = "A classic and refreshing cocktail.",
                ImageUrl = new Uri("https://www.example.com/old-fashioned.jpg"),
                Ingredients =
                [
                    new()
                    {
                        Name = "Bourbon",
                        Quantity = 2,
                        Unit = "oz"
                    },
                    new()
                    {
                        Name = "Angostura Bitters",
                        Quantity = 2,
                        Unit = "dashes"
                    },
                    new()
                    {
                        Name = "Simple Syrup",
                        Quantity = 1,
                        Unit = "tsp"
                    }
                ]
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Martini",
                Description = "A classic and refreshing cocktail.",
                ImageUrl = new Uri("https://www.example.com/martini.jpg"),
                Ingredients =
                [
                    new()
                    {
                        Name = "Gin",
                        Quantity = 2,
                        Unit = "oz"
                    },
                    new()
                    {
                        Name = "Dry Vermouth",
                        Quantity = 1,
                        Unit = "oz"
                    },
                    new()
                    {
                        Name = "Orange Bitters",
                        Quantity = 2,
                        Unit = "dashes"
                    }
                ]
            },
        };

        db!.AddRange(cocktails);
        db.SaveChanges();
        
        return services;
    }
}