namespace SmartLibrary.Application.DTOs.Recomendacoes;

public class RecomendacaoDto
{
    public int Id { get; set; }
    public int LivroId { get; set; }
    public string LivroTitulo { get; set; } = string.Empty;
    public string? LivroCapaUrl { get; set; }
    public string? LivroEditora { get; set; }
    public List<string> Autores { get; set; } = [];
    public decimal Score { get; set; }
    public string? Motivo { get; set; }
    public bool Visualizada { get; set; }
}
