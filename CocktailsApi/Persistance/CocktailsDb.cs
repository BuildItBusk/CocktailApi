using CocktailApi.Persistance.Models;
using Microsoft.EntityFrameworkCore;

namespace CocktailApi.Persistance;

public class CocktailsDb : DbContext
{
    public CocktailsDb(DbContextOptions<CocktailsDb> options) : base(options) {}

    public DbSet<Cocktail> Cocktails => Set<Cocktail>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ingredient>()
            .HasKey(i => i.Name)
            .HasName("PrimaryKey_Ingredient_Name");

        modelBuilder.Entity<Cocktail>()
            .HasKey(c => c.Id)
            .HasName("PrimaryKey_Cocktail_Id");
    }
}