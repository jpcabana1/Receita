using api.dto;
using api.model;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using Newtonsoft.Json;
using System.Net.Http;

namespace api.service;

public class SearchService : ISearchService
{
    private readonly SearchClient _srchclient;
    private readonly AppSettings _appSettings;
    
    public SearchService(AppSettings appSettings)
    {
        _appSettings = appSettings;
        _srchclient = new SearchClient(new Uri(_appSettings.AzureEndpoint), _appSettings.AzureIndex, new AzureKeyCredential(_appSettings.AzureApiKey));
    }

    public async Task<List<SearchDto>> SearchRecipesAsync(SearchRequestDto query, CancellationToken cancellationToken)
    {
        var options = new SearchOptions() {
            Select = {query.Select!},
            Filter = query.Filter,
            IncludeTotalCount = query.Count,
            Size = query.Top,
            Skip = query.Skip,
        };
        var result = await _srchclient.SearchAsync<SearchDto>(query.Search, options, cancellationToken);
        return result.Value
            .GetResults()
            .Select(s => s.Document)
            .ToList();
    }
}