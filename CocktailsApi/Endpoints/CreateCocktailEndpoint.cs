using CocktailApi.Contracts.Requests;
using CocktailApi.Persistance;
using CocktailApi.Persistance.Models;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace CocktailApi.Endpoints;

[HttpPost("/cocktails"), AllowAnonymous]
public sealed class CreateCocktailEndpoint : Endpoint<CreateCocktailRequest, CreateCocktailResponse> 
{
    private readonly CocktailsDb _db;

    public CreateCocktailEndpoint(CocktailsDb db)
    {
        _db = db;
    }
    
    public override async Task HandleAsync(CreateCocktailRequest request, CancellationToken cancellationToken)
    {
        var cocktail = new Cocktail 
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = "",
            Instructions = request.Recipe,
            Ingredients = request.Ingredients.Select(i => new Persistance.Models.Ingredient {
                Name = i.Name,
                Quantity = i.Quantity,
                Unit = i.Unit
            }).ToList(),
            ImageUrl =  new Uri(request.ImageUrl)
        };

        await _db.AddAsync(cocktail, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        await SendAsync(new CreateCocktailResponse(cocktail.Id), cancellation: cancellationToken);
    }
}