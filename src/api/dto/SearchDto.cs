using System.Text.Json.Serialization;
using Azure.Search.Documents.Indexes;

namespace api.model;


public partial class SearchDto
{
    [SearchableField(IsKey = true, IsFilterable = true)]
    [JsonPropertyName("Id")]
    public string? Id { get; set; }
    
    [SearchableField(IsKey = true, IsFilterable = true)]
    [JsonPropertyName("Nome")]
    public string? Nome { get; set; }
    
    [SearchableField(IsKey = true, IsFilterable = true)]
    [JsonPropertyName("Thumbnail")]
    public string? Thumbnail { get; set; }
    
    [SearchableField(IsKey = true, IsFilterable = true)]
    [JsonPropertyName("Descricao")]
    public string? Descricao { get; set; }
    
    [SearchableField(IsKey = true, IsFilterable = true)]
    [JsonPropertyName("Ingredientes")]
    public string? Ingredientes { get; set; }
    
    [SearchableField(IsKey = true, IsFilterable = true, IsFacetable = true)]
    [JsonPropertyName("ModoPreparo")]
    public string? ModoPreparo { get; set; }
    
    [SearchableField(IsKey = true, IsFilterable = true)]
    [JsonPropertyName("Tipo")]
    public string? Tipo { get; set; }
}
