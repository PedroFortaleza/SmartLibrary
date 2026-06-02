namespace SmartLibrary.Domain.Entities;

public class ParametroSistema
{
    public int Id { get; set; }
    public string Chave { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime AtualizadoEm { get; set; }
}
