using CocktailApi.Contracts.Requests;
using CocktailApi.Contracts.Responses;
using CocktailApi.Persistance;
using CocktailApi.Persistance.Models;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CocktailApi.Endpoints;

[HttpGet("/cocktails/{id:Guid}"), AllowAnonymous]
public sealed class GetCocktailEndpoint(CocktailsDb db) : Endpoint<GetCocktailRequest, CocktailResponse?>
{
    private readonly CocktailsDb _db = db;

    public override async Task HandleAsync(GetCocktailRequest request, CancellationToken cancellationToken)
    {
        Cocktail? cocktail = await _db.Cocktails
            .Include(c => c.Ingredients)
            .Where(c => c.Id == request.Id)
            .Select(c => c)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (cocktail is null)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        var response = new CocktailResponse(
            Id: cocktail.Id.ToString(),
            Name: cocktail.Name,
            Recipe: cocktail.Instructions,
            History: "",
            ImageUrl: cocktail.ImageUrl.ToString(),
            Ingredients: cocktail.Ingredients.Select(i => new Contracts.Responses.Ingredient(
                Name: i.Name,
                Quantity: i.Quantity,
                Unit: i.Unit
            )).ToList()
        );

        await SendAsync(response, cancellation: cancellationToken);
    }
}