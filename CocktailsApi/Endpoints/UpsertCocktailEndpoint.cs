using CocktailApi.Contracts;
using FastEndpoints;

namespace CocktailApi.Endpoints;

public sealed class UpsertCocktailEndpoint : Endpoint<UpsertCocktailRequest> 
{
    public override void Configure()
    {
        Put("/cocktails");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpsertCocktailRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}



public record IngredientReqeuest(
    string Name,
    string Measurement
);