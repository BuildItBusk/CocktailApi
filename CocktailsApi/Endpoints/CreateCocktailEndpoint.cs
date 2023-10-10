using CocktailApi.Contracts.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Cosmos;

namespace CocktailApi.Endpoints;

[HttpPost("/cocktails"), AllowAnonymous]
public sealed class CreateCocktailEndpoint : Endpoint<CreateCocktailRequest> 
{
    private readonly CosmosClient _client;

    public CreateCocktailEndpoint(CosmosClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }
    
    public override async Task HandleAsync(CreateCocktailRequest request, CancellationToken cancellationToken)
    {
        var database = _client.GetDatabase("Cocktails");
        var recipeContainer = database.GetContainer("Recipes");

        var cocktail = new CocktailModel(
            id: Guid.NewGuid().ToString(),
            name: request.Name,
            ingredients: request.Ingredients.Select(i => new IngredientModel(
                name: i.Name,
                quantity: i.Quantity,
                unit: i.Unit
            )).ToList(),
            recipe: request.Recipe,
            image: request.ImageUrl.ToString(),
            story: request.History,
            video: string.Empty
        );

        await recipeContainer.CreateItemAsync(
            item: cocktail,
            cancellationToken: cancellationToken);
    }
}