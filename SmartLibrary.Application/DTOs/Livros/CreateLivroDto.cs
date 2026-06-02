using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Application.DTOs.Livros;

public class CreateLivroDto
{
    [Required] [MaxLength(20)] public string ISBN { get; set; } = string.Empty;
    [Required] [MaxLength(300)] public string Titulo { get; set; } = string.Empty;
    [MaxLength(300)] public string? SubTitulo { get; set; }
    [MaxLength(150)] public string? Editora { get; set; }
    public int? AnoPublicacao { get; set; }
    [MaxLength(20)] public string? Edicao { get; set; }
    [MaxLength(50)] public string? Idioma { get; set; }
    public int? NumeroPaginas { get; set; }
    public string? Sinopse { get; set; }
    [MaxLength(500)] public string? CapaUrl { get; set; }
    public List<int> AutorIds { get; set; } = [];
    public List<int> CategoriaIds { get; set; } = [];
}

public class UpdateLivroDto : CreateLivroDto { }
