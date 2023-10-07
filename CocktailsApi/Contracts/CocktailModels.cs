namespace CocktailApi.Contracts;

public record CocktailResponse(
    string Name,
    string Recipe,
    Uri ImageUrl,
    string History,
    IEnumerable<Ingredient> Ingredients
);

public record UpsertCocktailRequest(
    string Name,
    string Recipe,
    Uri ImageUrl,
    string History,
    IEnumerable<Ingredient> Ingredients
);

public record Ingredient(
    string Name,
    decimal Quantity,
    string Unit
);