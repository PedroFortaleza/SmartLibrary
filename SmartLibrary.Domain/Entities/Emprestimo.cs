using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Domain.Entities;

public class Emprestimo
{
    public int Id { get; set; }
    public int ExemplarId { get; set; }
    public int AlunoId { get; set; }
    public int BibliotecarioId { get; set; }
    public DateTime DataEmprestimo { get; set; }
    public DateTime DataPrevistaDevolucao { get; set; }
    public DateTime? DataDevolucao { get; set; }
    public StatusEmprestimo Status { get; set; }
    public string? Observacao { get; set; }

    public Exemplar Exemplar { get; set; } = null!;
    public Aluno Aluno { get; set; } = null!;
    public Usuario Bibliotecario { get; set; } = null!;
    public ICollection<Renovacao> Renovacoes { get; set; } = [];
    public Multa? Multa { get; set; }
}
