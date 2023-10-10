using CocktailApi.Contracts;
using FastEndpoints;
using Microsoft.Azure.Cosmos;

namespace CocktailApi.Endpoints;

public sealed class UpsertCocktailEndpoint : Endpoint<UpsertCocktailRequest> 
{
    private readonly CosmosClient _client;

    public UpsertCocktailEndpoint(CosmosClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public override void Configure()
    {
        Put("/cocktails");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpsertCocktailRequest request, CancellationToken cancellationToken)
    {
        var database = _client.GetDatabase("Cocktails");
        var recipeContainer = database.GetContainer("Recipes");

        var cocktail = new CocktailModel(
            Id: Guid.NewGuid().ToString(),
            Name: request.Name,
            Ingredients: request.Ingredients.Select(i => new IngredientModel(
                Name: i.Name,
                Quantity: i.Quantity,
                Unit: i.Unit
            )).ToList(),
            Recipe: request.Recipe,
            Image: request.ImageUrl.ToString(),
            Story: request.History,
            Video: string.Empty
        );

        await recipeContainer.CreateItemAsync(
            item: cocktail,
            cancellationToken: cancellationToken);
    }
}