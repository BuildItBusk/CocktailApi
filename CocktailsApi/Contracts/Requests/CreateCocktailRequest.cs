namespace CocktailApi.Contracts.Requests;

public record CreateCocktailRequest(
    string Name,
    string Recipe,
    string ImageUrl,
    string History,
    List<Ingredient> Ingredients
);

public record Ingredient(
    string Name,
    decimal Quantity,
    string Unit
);
