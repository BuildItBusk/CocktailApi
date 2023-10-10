namespace CocktailApi.Contracts.Responses;

public record CocktailResponse(
    string Id,
    string Name,
    string Recipe,
    string History,
    string ImageUrl,
    List<Ingredient> Ingredients
);

public record Ingredient(
    string Name,
    decimal Quantity,
    string Unit
);