using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Application.DTOs.Parametros;

public class ParametroDto
{
    public int Id { get; set; }
    public string Chave { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime AtualizadoEm { get; set; }
}

public class UpdateParametroDto
{
    [Required] [MaxLength(300)] public string Valor { get; set; } = string.Empty;
}
