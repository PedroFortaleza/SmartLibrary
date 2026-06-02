using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Domain.Entities;

public class Reserva
{
    public int Id { get; set; }
    public int LivroId { get; set; }
    public int AlunoId { get; set; }
    public DateTime DataReserva { get; set; }
    public DateTime DataExpiracao { get; set; }
    public StatusReserva Status { get; set; }
    public DateTime? NotificadoEm { get; set; }

    public Livro Livro { get; set; } = null!;
    public Aluno Aluno { get; set; } = null!;
}
