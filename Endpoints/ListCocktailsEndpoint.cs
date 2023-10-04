using CocktailApi.Contracts;
using FastEndpoints;

namespace CocktailApi.Endpoints;

public sealed class ListCocktailsEndpoint : EndpointWithoutRequest<List<CocktailResponse>>
{
    public override void Configure()
    {
        Get("/cocktails");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        Response = new List<CocktailResponse>
        {
            new(
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
            ),
            new(
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
            ),
            new(
                "Mojito",
                "A delicious cocktail",
                new Uri("https://www.thecocktaildb.com/images/media/drink/5noda61589575158.jpg"),
                "Mix it all together",
                new List<Ingredient>
                {
                    new("Rum", 1, "oz"),
                    new("Lime juice", 1, "oz"),
                    new("Mint", 1, "sprig"),
                    new("Sugar", 1, "pinch")
                }
            )
        };

        await Task.CompletedTask;
    }
}