using Microsoft.EntityFrameworkCore;

namespace api.model;

public class RecipeContext : DbContext
{
    public DbSet<Login> Users { get; set; }
    public DbSet<Recipe> Recipes { get; set; }

    public RecipeContext(DbContextOptions<RecipeContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { 
        modelBuilder.Entity<Login>().ToTable("Login");
        modelBuilder.Entity<Recipe>().ToTable("Recipe");
    }
}