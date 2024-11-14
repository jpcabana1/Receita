using System.Text.Json.Serialization;

namespace api.dto;

public class SearchRequestDto
{
    [JsonPropertyName("search")]
    public string? Search { get; set; } = "";

    [JsonPropertyName("select")]
    public string? Select { get; set; } = "";

    [JsonPropertyName("filter")]
    public string? Filter { get; set; } = "";

    [JsonPropertyName("count")]
    public bool Count { get; set; } = true;

    [JsonPropertyName("top")]
    public int Top { get; set; } = 50;

    [JsonPropertyName("skip")]
    public int Skip { get; set; } = 0;
}