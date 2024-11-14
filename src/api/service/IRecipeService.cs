using api.dto;

namespace api.service;

public interface IRecipeService
{
    void AddRecipes(List<RecipeDto> recipes);
}