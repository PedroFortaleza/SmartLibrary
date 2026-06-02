using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Domain.Entities;

public class Notificacao
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public TipoNotificacao Tipo { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public bool Lida { get; set; } = false;
    public DateTime CriadaEm { get; set; }

    public Usuario Usuario { get; set; } = null!;
}
