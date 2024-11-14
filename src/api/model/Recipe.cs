using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.model;
public class Recipe
{
    [Key]
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Thumbnail { get; set; }
    public string? Descricao { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? Ingredientes { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? ModoPreparo { get; set; }
    public string Tipo { get; set; } = null!;
}