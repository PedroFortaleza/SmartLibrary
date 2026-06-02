using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Domain.Entities;

public class Aluno
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string Matricula { get; set; } = string.Empty;
    public string Curso { get; set; } = string.Empty;
    public TurnoAluno Turno { get; set; }
    public string? Cep { get; set; }
    public string? Logradouro { get; set; }
    public string? Cidade { get; set; }
    public string? UF { get; set; }
    public DateOnly? DataNascimento { get; set; }
    public string? Telefone { get; set; }

    public Usuario Usuario { get; set; } = null!;
    public ICollection<Emprestimo> Emprestimos { get; set; } = [];
    public ICollection<Reserva> Reservas { get; set; } = [];
    public ICollection<Recomendacao> Recomendacoes { get; set; } = [];
    public ICollection<Avaliacao> Avaliacoes { get; set; } = [];
}
