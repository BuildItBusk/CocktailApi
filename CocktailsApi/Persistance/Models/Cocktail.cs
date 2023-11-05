namespace CocktailApi.Persistance.Models;

public class Cocktail
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public string Instructions { get; init; } = "";
    public List<Ingredient> Ingredients { get; init; } = new List<Ingredient>();
    // public List<string> Tags { get; init; } = new List<string>();
    public Uri ImageUrl { get; init; } = new Uri("https://via.placeholder.com/150");
}

public class Ingredient
{
    public string Name { get; init; } = "";
    public decimal Quantity { get; init; }
    public string Unit { get; init; } = "";
}
