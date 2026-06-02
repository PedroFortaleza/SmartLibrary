namespace SmartLibrary.Domain.Entities;

public class LivroAutor
{
    public int LivroId { get; set; }
    public int AutorId { get; set; }
    public int Ordem { get; set; } = 1;

    public Livro Livro { get; set; } = null!;
    public Autor Autor { get; set; } = null!;
}
