namespace SmartLibrary.Domain.Entities;

public class Livro
{
    public int Id { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string? SubTitulo { get; set; }
    public string? Editora { get; set; }
    public int? AnoPublicacao { get; set; }
    public string? Edicao { get; set; }
    public string? Idioma { get; set; }
    public int? NumeroPaginas { get; set; }
    public string? Sinopse { get; set; }
    public string? CapaUrl { get; set; }
    public string? GoogleBooksId { get; set; }
    public string? OpenLibraryId { get; set; }
    public DateTime CriadoEm { get; set; }

    public ICollection<LivroAutor> LivroAutores { get; set; } = [];
    public ICollection<LivroCategoria> LiveCategorias { get; set; } = [];
    public ICollection<Exemplar> Exemplares { get; set; } = [];
    public ICollection<Reserva> Reservas { get; set; } = [];
    public ICollection<Recomendacao> Recomendacoes { get; set; } = [];
    public ICollection<Avaliacao> Avaliacoes { get; set; } = [];
}
