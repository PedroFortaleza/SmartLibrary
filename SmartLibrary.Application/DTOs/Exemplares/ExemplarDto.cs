using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Application.DTOs.Exemplares;

public class ExemplarDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string? Localizacao { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public bool Ativo { get; set; }
}

public class CreateExemplarDto
{
    [Required] public int LivroId { get; set; }
    [Required] [MaxLength(50)] public string Codigo { get; set; } = string.Empty;
    [MaxLength(100)] public string? Localizacao { get; set; }
    [Required] public string Tipo { get; set; } = string.Empty;
}

public class UpdateEstadoExemplarDto
{
    [Required] public string Estado { get; set; } = string.Empty;
}
