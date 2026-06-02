namespace SmartLibrary.Application.DTOs.Livros;

public class LivroListDto
{
    public int Id { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string? SubTitulo { get; set; }
    public string? Editora { get; set; }
    public int? AnoPublicacao { get; set; }
    public string? CapaUrl { get; set; }
    public List<string> Autores { get; set; } = [];
    public List<string> Categorias { get; set; } = [];
    public int TotalExemplares { get; set; }
    public int ExemplaresDisponiveis { get; set; }
    public double? MediaAvaliacao { get; set; }
}
