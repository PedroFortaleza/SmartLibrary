using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public RoleUsuario Role { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime CriadoEm { get; set; }
    public DateTime? UltimoLogin { get; set; }

    public Aluno? Aluno { get; set; }
    public ICollection<Notificacao> Notificacoes { get; set; } = [];
    public ICollection<LogAcao> LogAcoes { get; set; } = [];
}
