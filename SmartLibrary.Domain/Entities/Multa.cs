using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Domain.Entities;

public class Multa
{
    public int Id { get; set; }
    public int EmprestimoId { get; set; }
    public decimal ValorDiario { get; set; }
    public int DiasAtraso { get; set; }
    public decimal ValorTotal { get; set; }
    public StatusMulta Status { get; set; }
    public DateTime? DataPagamento { get; set; }
    public FormaPagamento? FormaPagamento { get; set; }

    public Emprestimo Emprestimo { get; set; } = null!;
}
