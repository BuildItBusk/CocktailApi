using System.Net;
using CocktailApi.Contracts.Requests;
using CocktailApi.Contracts.Responses;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Cosmos;

namespace CocktailApi.Endpoints;

[HttpGet("/cocktails/{id:Guid}"), AllowAnonymous]
public sealed class GetCocktailEndpoint : Endpoint<GetCocktailRequest, CocktailResponse?>
{
    private readonly CosmosClient _client;

    public GetCocktailEndpoint(CosmosClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public override async Task HandleAsync(GetCocktailRequest request, CancellationToken cancellationToken)
    {
        var database = _client.GetDatabase("Cocktails");
        var container = database.GetContainer("Recipes");

        try 
        {
            CocktailModel item = await container.ReadItemAsync<CocktailModel>(
                id: request.Id.ToString(), 
                partitionKey: new PartitionKey(request.Id.ToString()), 
                cancellationToken: cancellationToken);
            
            var cocktail = new CocktailResponse(
                Id: item.id,
                Name: item.name,
                Recipe: item.recipe,
                History: item.story,
                ImageUrl: item.image,
                Ingredients: item.ingredients.Select(i => new Contracts.Responses.Ingredient(
                    Name: i.name,
                    Quantity: i.quantity,
                    Unit: i.unit
                )).ToList()
            );

            await SendAsync(cocktail, cancellation: cancellationToken);
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }
    }
}