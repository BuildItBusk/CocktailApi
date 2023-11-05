using CocktailApi.Contracts.Responses;
using CocktailApi.Persistance;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CocktailApi.Endpoints;

[HttpGet("/cocktails"), AllowAnonymous]
public sealed class ListCocktailsEndpoint : EndpointWithoutRequest<List<CocktailResponse>>
{
    private readonly CocktailsDb _db;

    public ListCocktailsEndpoint(CocktailsDb db)
    {
        _db = db;
    }
    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var cocktails = await _db.Cocktails
            .Include(c => c.Ingredients)
            .ToListAsync(cancellationToken);

        Response = cocktails.Select(c => new CocktailResponse(
            Id: c.Id.ToString(),
            Name: c.Name,
            Recipe: c.Instructions,
            History: "",
            ImageUrl: c.ImageUrl.ToString(),
            Ingredients: c.Ingredients.Select(i => new Ingredient(
                Name: i.Name,
                Quantity: i.Quantity,
                Unit: i.Unit
            )).ToList()
        )).ToList();
    }
}