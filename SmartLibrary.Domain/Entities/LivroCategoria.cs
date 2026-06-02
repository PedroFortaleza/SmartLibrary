namespace SmartLibrary.Domain.Entities;

public class LivroCategoria
{
    public int LivroId { get; set; }
    public int CategoriaId { get; set; }

    public Livro Livro { get; set; } = null!;
    public Categoria Categoria { get; set; } = null!;
}
