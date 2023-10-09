using Azure.Identity;
using CocktailApi.Contracts;
using FastEndpoints;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace CocktailApi.Endpoints;

public sealed class UpsertCocktailEndpoint : Endpoint<UpsertCocktailRequest> 
{
    private readonly IOptionsSnapshot<CosmosDbOptions> _cosmosDbOptions;

    public UpsertCocktailEndpoint(IOptionsSnapshot<CosmosDbOptions> cosmosDbOptions)
    {
        _cosmosDbOptions = cosmosDbOptions;
    }

    public override void Configure()
    {
        Put("/cocktails");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpsertCocktailRequest request, CancellationToken cancellationToken)
    {
        using CosmosClient client = new(
            accountEndpoint: _cosmosDbOptions.Value.Uri,
            tokenCredential: new DefaultAzureCredential()
        );

        var database = client.GetDatabase("Cocktails");
        var recipeContainer = database.GetContainer("Recipes");

        var cocktail = new CocktailDocument
        {
            id = Guid.NewGuid().ToString(),
            name = request.Name,
            ingredients = request.Ingredients.Select(i => new IngredientDocument
            {
                name = i.Name,
                quantity = i.Quantity,
                unit = i.Unit
            }).ToList(),
            recipe = request.Recipe,
            image = request.ImageUrl.ToString(),
            story = request.History,
            video = string.Empty
        };

        await recipeContainer.CreateItemAsync(
            item: cocktail,
            cancellationToken: cancellationToken);
    }
}

internal class CocktailDocument
{
    public string id { get; set; }
    public string name { get; set; }
    public List<IngredientDocument> ingredients { get; set; }
    public string recipe { get; set; }
    public string image { get; set; }
    public string story { get; set; }
    public string video { get; set; }
}

internal class IngredientDocument
{
    public string name { get; set; }
    public decimal quantity { get; set; }
    public string unit { get; set; }
}
