namespace CocktailApi;

internal record CocktailModel(
    string id,
    string name,
    List<IngredientModel> ingredients,
    string recipe,
    string image,
    string story,
    string video
);

internal record IngredientModel(
    string name,
    decimal quantity,
    string unit
);