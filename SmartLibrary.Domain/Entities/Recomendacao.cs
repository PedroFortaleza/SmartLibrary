namespace SmartLibrary.Domain.Entities;

public class Recomendacao
{
    public int Id { get; set; }
    public int AlunoId { get; set; }
    public int LivroId { get; set; }
    public decimal Score { get; set; }
    public string? Motivo { get; set; }
    public bool Visualizada { get; set; } = false;
    public DateTime CriadaEm { get; set; }

    public Aluno Aluno { get; set; } = null!;
    public Livro Livro { get; set; } = null!;
}
