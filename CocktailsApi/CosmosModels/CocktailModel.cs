namespace CocktailApi;

internal record CocktailModel(
    string Id,
    string Name,
    List<IngredientModel> Ingredients,
    string Recipe,
    string Image,
    string Story,
    string Video
);

internal record IngredientModel(
    string Name,
    decimal Quantity,
    string Unit
);