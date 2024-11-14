using api.dto;
using api.model;

namespace api.service;

public interface ISearchService{
    Task<List<SearchDto>> SearchRecipesAsync(SearchRequestDto query, CancellationToken cancellationToken);
}