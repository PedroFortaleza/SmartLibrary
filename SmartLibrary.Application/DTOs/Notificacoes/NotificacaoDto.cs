namespace SmartLibrary.Application.DTOs.Notificacoes;

public class NotificacaoDto
{
    public int Id { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public bool Lida { get; set; }
    public DateTime CriadaEm { get; set; }
}
