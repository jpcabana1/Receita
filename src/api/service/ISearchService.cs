using api.dto;
using api.model;
using Azure.Search.Documents.Models;

namespace api.service;

public interface ISearchService{
    Task<List<SearchDto>> SearchRecipesAsync(SearchRequestDto query, CancellationToken cancellationToken);
}