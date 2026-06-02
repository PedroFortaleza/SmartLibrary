namespace SmartLibrary.Application.DTOs.Autores;

public class AutorDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Nacionalidade { get; set; }
    public string? Biografia { get; set; }
}
