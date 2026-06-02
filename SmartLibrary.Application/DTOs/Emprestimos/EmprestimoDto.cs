using System.ComponentModel.DataAnnotations;
using SmartLibrary.Application.DTOs.Multas;

namespace SmartLibrary.Application.DTOs.Emprestimos;

public class EmprestimoDto
{
    public int Id { get; set; }
    public int ExemplarId { get; set; }
    public string ExemplarCodigo { get; set; } = string.Empty;
    public string LivroTitulo { get; set; } = string.Empty;
    public int LivroId { get; set; }
    public int AlunoId { get; set; }
    public string AlunoNome { get; set; } = string.Empty;
    public string BibliotecarioNome { get; set; } = string.Empty;
    public DateTime DataEmprestimo { get; set; }
    public DateTime DataPrevistaDevolucao { get; set; }
    public DateTime? DataDevolucao { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Observacao { get; set; }
    public int TotalRenovacoes { get; set; }
    public MultaDto? Multa { get; set; }
}

public class CreateEmprestimoDto
{
    [Required] public int ExemplarId { get; set; }
    [Required] public int AlunoId { get; set; }
    [MaxLength(500)] public string? Observacao { get; set; }
}
