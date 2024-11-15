using api.dto;
using api.model;

namespace api.service;

public interface ISearchService{
    Task<List<RecipeDto>> SearchRecipesAsync(SearchRequestDto query, CancellationToken cancellationToken);
}