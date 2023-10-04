using CocktailApi.Contracts;
using FastEndpoints;

namespace CocktailApi.Endpoints;

public sealed class GetCocktailEndpoint : Endpoint<CocktailRequest, CocktailResponse>
{
    public override void Configure()
    {
        Get("cocktails/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CocktailRequest request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Getting cocktail with id {request.Id}");

        Response = new CocktailResponse(
            "Margarita",
            "A delicious cocktail",
            new Uri("https://www.thecocktaildb.com/images/media/drink/5noda61589575158.jpg"),
            "Mix it all together",
            new List<Ingredient>
            {
                new("Tequila", 1, "oz"),
                new("Lime juice", 1, "oz"),
                new("Cointreau", 1, "oz"),
                new("Salt", 1, "pinch")
            }
        );

        await Task.CompletedTask;
    }
}

public class CocktailRequest
{
    public Guid Id { get; init; }
}