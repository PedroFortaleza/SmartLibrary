namespace SmartLibrary.Domain.Entities;

public class Avaliacao
{
    public int Id { get; set; }
    public int LivroId { get; set; }
    public int AlunoId { get; set; }
    public int Nota { get; set; }
    public string? Comentario { get; set; }
    public DateTime CriadaEm { get; set; }
    public bool Aprovada { get; set; } = false;

    public Livro Livro { get; set; } = null!;
    public Aluno Aluno { get; set; } = null!;
}
