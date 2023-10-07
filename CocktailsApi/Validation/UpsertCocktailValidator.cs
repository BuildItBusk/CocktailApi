
using CocktailApi.Contracts;
using FastEndpoints;
using FluentValidation;

namespace CocktailApi.Validation;

public sealed class UpsertCocktailValidator : Validator<UpsertCocktailRequest>
{
    public UpsertCocktailValidator()
    {
        RuleFor(req => req.Name).NotEmpty();
        RuleFor(req => req.Recipe).NotEmpty();
        RuleFor(req => req.ImageUrl).NotEmpty().Must(BeValidUri).WithMessage("ImageUrl must be a valid Uri.");
        RuleFor(req => req.History).NotEmpty();
        RuleFor(req => req.Ingredients).NotEmpty().Must(HaveAtLeastTwoItems);
        RuleForEach(req => req.Ingredients).ChildRules(ingredient =>
        {
            ingredient.RuleFor(req => req.Name).NotEmpty();
            ingredient.RuleFor(req => req.Quantity).GreaterThan(0);
            ingredient.RuleFor(req => req.Unit).NotEmpty();
        });
    }

    private bool HaveAtLeastTwoItems(IEnumerable<Ingredient> enumerable)
    {
        return enumerable != null && enumerable.Count() >= 2;
    }

    private bool BeValidUri(Uri uri)
    {
        return Uri.TryCreate(uri.ToString(), UriKind.Absolute, out _);
    }
}