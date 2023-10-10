using System.Text.Json;
using CocktailApi;
using CocktailApi.Contracts.Responses;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Tests.Integration;

public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public IntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public void ListCocktails()
    {
        var client = _factory.CreateClient();

        var response = client.GetAsync("/cocktails").Result;

        response.EnsureSuccessStatusCode();

        var content = response.Content.ReadAsStringAsync().Result;
        var cocktails = JsonSerializer.Deserialize<List<CocktailResponse>>(content);

        Assert.NotNull(cocktails);
        Assert.NotEmpty(cocktails);
    }
}