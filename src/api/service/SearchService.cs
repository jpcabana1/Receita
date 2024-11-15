using api.dto;
using api.model;
using AutoMapper;
using Azure;
using Azure.Search.Documents;

namespace api.service;

public class SearchService : ISearchService
{
    private readonly SearchClient _srchclient;
    private readonly AppSettings _appSettings;
    private readonly IMapper _mapper;

    public SearchService(AppSettings appSettings, IMapper mapper)
    {
        _appSettings = appSettings;
        _srchclient = new SearchClient(new Uri(_appSettings.AzureEndpoint), _appSettings.AzureIndex, new AzureKeyCredential(_appSettings.AzureApiKey));
        _mapper = mapper;
    }

    public async Task<List<RecipeDto>> SearchRecipesAsync(SearchRequestDto query, CancellationToken cancellationToken)
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
            .Select(s => _mapper.Map<RecipeDto>(s.Document))
            .ToList();
    }
}