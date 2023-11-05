using System.Net.Http.Json;
using System.Text.Json;
using CocktailApi;
using CocktailApi.Contracts.Requests;
using CocktailApi.Contracts.Responses;

namespace Tests.Integration;

public class IntegrationTests(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory = factory;
    private readonly JsonSerializerOptions _caseInsensitiveOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    [Fact]
    public async Task GetCocktail()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/cocktails/4b54e698-706e-4fd9-a9e4-94407c2ed735");

        response.EnsureSuccessStatusCode();

        var content = response.Content.ReadAsStringAsync().Result;
        var cocktail = JsonSerializer.Deserialize<CocktailResponse>(content, _caseInsensitiveOptions);

        Assert.NotNull(cocktail);
        Assert.Equal("Margarita", cocktail.Name);
        Assert.NotEmpty(cocktail.Ingredients);
    }

    [Fact]
    public async Task ListCocktails()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/cocktails");

        response.EnsureSuccessStatusCode();

        var content = response.Content.ReadAsStringAsync().Result;
        var cocktails = JsonSerializer.Deserialize<List<CocktailResponse>>(content, _caseInsensitiveOptions);

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
            Ingredients:
            [
                new("Test Ingredient", 1, "Test Unit"),
                new("Test Ingredient 2", 2, "Test Unit 2"),
                new("Test Ingredient 3", 3, "Test Unit 3")
            ]
        );

        var response = await client.PostAsJsonAsync("/cocktails", cocktail);

        response.EnsureSuccessStatusCode();       

        var content = response.Content.ReadAsStringAsync().Result;
        var createdCocktail = JsonSerializer.Deserialize<CreateCocktailResponse>(content, _caseInsensitiveOptions);
    
        Assert.NotNull(createdCocktail);
        Assert.NotEqual(Guid.Empty, createdCocktail.Id);
    }
}