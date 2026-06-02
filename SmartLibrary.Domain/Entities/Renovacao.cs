namespace SmartLibrary.Domain.Entities;

public class Renovacao
{
    public int Id { get; set; }
    public int EmprestimoId { get; set; }
    public DateTime DataRenovacao { get; set; }
    public DateTime NovaDataPrevista { get; set; }
    public int UsuarioId { get; set; }

    public Emprestimo Emprestimo { get; set; } = null!;
    public Usuario Usuario { get; set; } = null!;
}
