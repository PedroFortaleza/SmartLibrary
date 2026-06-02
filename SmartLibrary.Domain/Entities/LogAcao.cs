namespace SmartLibrary.Domain.Entities;

public class LogAcao
{
    public long Id { get; set; }
    public int UsuarioId { get; set; }
    public string Acao { get; set; } = string.Empty;
    public string? Entidade { get; set; }
    public int? EntidadeId { get; set; }
    public string? Detalhe { get; set; }
    public DateTime CriadoEm { get; set; }

    public Usuario Usuario { get; set; } = null!;
}
