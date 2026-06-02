using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Application.DTOs.Reservas;

public class ReservaDto
{
    public int Id { get; set; }
    public int LivroId { get; set; }
    public string LivroTitulo { get; set; } = string.Empty;
    public string? LivroCapaUrl { get; set; }
    public int AlunoId { get; set; }
    public string AlunoNome { get; set; } = string.Empty;
    public DateTime DataReserva { get; set; }
    public DateTime DataExpiracao { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? NotificadoEm { get; set; }
}

public class CreateReservaDto
{
    [Required] public int LivroId { get; set; }
}
