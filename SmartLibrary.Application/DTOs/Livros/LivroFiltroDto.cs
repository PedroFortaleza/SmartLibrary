namespace SmartLibrary.Application.DTOs.Livros;

public class LivroFiltroDto
{
    public string? Search { get; set; }
    public int? CategoriaId { get; set; }
    public int? AutorId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
