namespace SmartLibrary.Application.DTOs.External;

public class GoogleBooksResultDto
{
    public string? Titulo { get; set; }
    public string? SubTitulo { get; set; }
    public List<string> Autores { get; set; } = [];
    public string? Editora { get; set; }
    public string? Descricao { get; set; }
    public string? CapaUrl { get; set; }
    public int? NumeroPaginas { get; set; }
    public string? Idioma { get; set; }
    public int? AnoPublicacao { get; set; }
    public string? GoogleBooksId { get; set; }
}
