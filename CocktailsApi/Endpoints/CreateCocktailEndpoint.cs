using CocktailApi.Contracts.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Cosmos;

namespace CocktailApi.Endpoints;

[HttpPost("/cocktails"), AllowAnonymous]
public sealed class CreateCocktailEndpoint : Endpoint<CreateCocktailRequest, CreateCocktailResponse> 
{
    private readonly CosmosClient _client;

    public CreateCocktailEndpoint(CosmosClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }
    
    public override async Task HandleAsync(CreateCocktailRequest request, CancellationToken cancellationToken)
    {
        Database database = _client.GetDatabase("Cocktails");
        Container container = database.GetContainer("Recipes");
        
        if (await CocktailExists(container, request.Name))
        {
            HttpContext.Response.StatusCode = StatusCodes.Status409Conflict;
            await HttpContext.Response.WriteAsync($"A cocktail named '{request.Name}' already exists.", cancellationToken);
            return;
        }

        Guid Id = Guid.NewGuid();
        var cocktail = new CocktailModel(
            id: Id.ToString(),
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

        await container.CreateItemAsync(
            item: cocktail,
            cancellationToken: cancellationToken);

        await SendAsync(new CreateCocktailResponse(Id), cancellation: cancellationToken);
    }

    private static async Task<bool> CocktailExists(Container container, string cocktailName)
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.name = @name")
            .WithParameter("@name", cocktailName);

        var iterator = container.GetItemQueryIterator<CocktailModel>(query);
        if (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();            
            return response.Any();
        }

        return false;
    }
}