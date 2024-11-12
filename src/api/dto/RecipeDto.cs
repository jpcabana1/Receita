using System.Text.Json.Serialization;

namespace api.dto;

public class RecipeDto
{
    [JsonPropertyName("nome")]
    public required string Nome { get; set; }

    [JsonPropertyName("thumbnail")]
    public required string Thumbnail { get; set; }

    [JsonPropertyName("descricao")]
    public required string Descricao { get; set; }

    [JsonPropertyName("ingredientes")]
    public List<string> Ingredientes { get; set; } = [];

    [JsonPropertyName("modo_preparo")]
    public List<string> ModoPreparo { get; set; } = [];

    [JsonPropertyName("tipo")]
    public required string Tipo { get; set; }
}