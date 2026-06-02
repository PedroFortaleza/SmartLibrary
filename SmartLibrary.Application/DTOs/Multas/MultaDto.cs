using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Application.DTOs.Multas;

public class MultaDto
{
    public int Id { get; set; }
    public int EmprestimoId { get; set; }
    public decimal ValorDiario { get; set; }
    public int DiasAtraso { get; set; }
    public decimal ValorTotal { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? DataPagamento { get; set; }
    public string? FormaPagamento { get; set; }
}

public class PagarMultaDto
{
    [Required] public string FormaPagamento { get; set; } = string.Empty;
}
