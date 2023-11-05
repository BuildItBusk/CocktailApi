using CocktailApi.Contracts.Requests;
using CocktailApi.Validation;
using FluentValidation.TestHelper;

namespace Tests.Unit;

public class Validation
{
    private readonly CreateCocktailValidator _validator;

    public Validation()
    {
        _validator = new CreateCocktailValidator();
    }

    [Fact]
    public void ValidRequest()
    {
        var ingredients = new List<Ingredient>
        {
            new("Rum", 1, "oz"),
            new("Lime Juice", 1, "oz"),
            new("Simple Syrup", 1, "oz"),
            new("Mint Leaves", 4, "leaves"),
            new("Club Soda", 2, "oz")
        };

        var cocktail = new CreateCocktailRequest(
            "Mojito", 
            "Do stuff", 
            "https://www.thecocktaildb.com/images/media/drink/3z6xdi1589574603.jpg",
            "Something",
            ingredients);

        var result = _validator.TestValidate(cocktail);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void TooFewIngredients()
    {
        var ingredients = new List<Ingredient>
        {
            new("Rum", 1, "oz")
        };

        var cocktail = new CreateCocktailRequest(
            "Mojito", 
            "Do stuff", 
            "https://www.thecocktaildb.com/images/media/drink/3z6xdi1589574603.jpg",
            "Something",
            ingredients);

        var result = _validator.TestValidate(cocktail);

        result.ShouldHaveValidationErrorFor(x => x.Ingredients);
    }
}