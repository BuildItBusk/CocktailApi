using CocktailApi.Contracts;
using FastEndpoints;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace CocktailApi.Endpoints;

public sealed class ListCocktailsEndpoint : EndpointWithoutRequest<List<CocktailResponse>>
{
    private readonly CosmosClient _client;

    public ListCocktailsEndpoint(CosmosClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public override void Configure()
    {
        Get("/cocktails");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var database = _client.GetDatabase("Cocktails");
        var recipeContainer = database.GetContainer("Recipes");
        
        var iterator = recipeContainer.GetItemLinqQueryable<CocktailModel>().ToFeedIterator();

        var cocktails = new List<CocktailResponse>();
        while (iterator.HasMoreResults)
        {
            foreach(var item in await iterator.ReadNextAsync(cancellationToken))
            {
                cocktails.Add(new CocktailResponse(
                    Name: item.Name,
                    Ingredients: item.Ingredients.Select(i => new Ingredient(
                        Name: i.Name,
                        Quantity: i.Quantity,
                        Unit: i.Unit
                    )).ToList(),
                    Recipe: item.Recipe,
                    ImageUrl: new Uri("https://www.test.com/cocktail.jpg"),
                    History: item.Story
                ));
            }
        }

        Response = cocktails;
    }
}