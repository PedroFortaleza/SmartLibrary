using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Application.DTOs.Avaliacoes;

public class AvaliacaoDto
{
    public int Id { get; set; }
    public int LivroId { get; set; }
    public string LivroTitulo { get; set; } = string.Empty;
    public int AlunoId { get; set; }
    public string AlunoNome { get; set; } = string.Empty;
    public int Nota { get; set; }
    public string? Comentario { get; set; }
    public DateTime CriadaEm { get; set; }
    public bool Aprovada { get; set; }
}

public class CreateAvaliacaoDto
{
    [Required] public int LivroId { get; set; }
    [Required] [Range(1, 5)] public int Nota { get; set; }
    public string? Comentario { get; set; }
}
