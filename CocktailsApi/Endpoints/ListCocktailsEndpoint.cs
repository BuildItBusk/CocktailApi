using CocktailApi.Contracts.Responses;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace CocktailApi.Endpoints;

[HttpGet("/cocktails"), AllowAnonymous]
public sealed class ListCocktailsEndpoint : EndpointWithoutRequest<List<CocktailResponse>>
{
    private readonly CosmosClient _client;

    public ListCocktailsEndpoint(CosmosClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
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
                    Id: item.id,
                    Name: item.name,
                    Ingredients: item.ingredients.Select(i => new Ingredient(
                        Name: i.name,
                        Quantity: i.quantity,
                        Unit: i.unit
                    )).ToList(),
                    Recipe: item.recipe,
                    ImageUrl: item.image,
                    History: item.story
                ));
            }
        }

        Response = cocktails;
    }
}