using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Domain.Entities;

public class Exemplar
{
    public int Id { get; set; }
    public int LivroId { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string? Localizacao { get; set; }
    public TipoExemplar Tipo { get; set; }
    public EstadoExemplar Estado { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime CriadoEm { get; set; }

    public Livro Livro { get; set; } = null!;
    public ICollection<Emprestimo> Emprestimos { get; set; } = [];
}
