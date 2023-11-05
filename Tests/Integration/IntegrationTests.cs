using System.Net.Http.Json;
using System.Text.Json;
using CocktailApi;
using CocktailApi.Contracts.Requests;
using CocktailApi.Contracts.Responses;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Integration;

public class IntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public IntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ListCocktails()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/cocktails");

        response.EnsureSuccessStatusCode();

        var content = response.Content.ReadAsStringAsync().Result;
        var cocktails = JsonSerializer.Deserialize<List<CocktailResponse>>(content);

        Assert.NotNull(cocktails);
        Assert.NotEmpty(cocktails);
    }

    [Fact]
    public async Task CreateCocktail()
    {
        var client = _factory.CreateClient();

        var cocktail = new CreateCocktailRequest(
            Name: "Test Cocktail",
            Recipe: "Test Recipe",
            ImageUrl: "https://www.test.com/test.jpg",
            History: "Test History",
            Ingredients: new List<CocktailApi.Contracts.Requests.Ingredient>
            {
                new("Test Ingredient", 1, "Test Unit"),
                new("Test Ingredient 2", 2, "Test Unit 2"),
                new("Test Ingredient 3", 3, "Test Unit 3")
            }
        );

        var response = await client.PostAsJsonAsync("/cocktails", cocktail);

        response.EnsureSuccessStatusCode();

        var content = response.Content.ReadAsStringAsync().Result;
        var createdCocktail = JsonSerializer.Deserialize<CreateCocktailResponse>(content);
    
        Assert.NotNull(createdCocktail);
        Assert.NotEqual(Guid.Empty, createdCocktail.Id);
    }
}