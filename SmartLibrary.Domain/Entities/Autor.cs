namespace SmartLibrary.Domain.Entities;

public class Autor
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Nacionalidade { get; set; }
    public string? Biografia { get; set; }

    public ICollection<LivroAutor> LivroAutores { get; set; } = [];
}
