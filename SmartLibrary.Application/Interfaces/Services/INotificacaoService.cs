using SmartLibrary.Application.DTOs.Notificacoes;

namespace SmartLibrary.Application.Interfaces.Services;

public interface INotificacaoService
{
    Task<List<NotificacaoDto>> GetByUsuarioAsync(int usuarioId);
    Task MarcarLidaAsync(int id, int usuarioId);
    Task MarcarTodasLidasAsync(int usuarioId);
}
