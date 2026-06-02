using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Notificacoes;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Application.Interfaces.Services;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Application.Services;

public class NotificacaoService(IBaseRepository<Notificacao> notificacaoRepo) : INotificacaoService
{
    public async Task<List<NotificacaoDto>> GetByUsuarioAsync(int usuarioId)
    {
        var todas = await notificacaoRepo.GetAllAsync();
        return todas.Where(n => n.UsuarioId == usuarioId)
            .OrderByDescending(n => n.CriadaEm)
            .Select(n => new NotificacaoDto
            {
                Id = n.Id,
                Tipo = n.Tipo.ToString(),
                Mensagem = n.Mensagem,
                Lida = n.Lida,
                CriadaEm = n.CriadaEm
            }).ToList();
    }

    public async Task MarcarLidaAsync(int id, int usuarioId)
    {
        var notificacao = await notificacaoRepo.GetByIdAsync(id)
            ?? throw new NotFoundException("Notificação não encontrada.");

        if (notificacao.UsuarioId != usuarioId)
            throw new BusinessException("Sem permissão para esta notificação.");

        notificacao.Lida = true;
        await notificacaoRepo.UpdateAsync(notificacao);
    }

    public async Task MarcarTodasLidasAsync(int usuarioId)
    {
        var todas = await notificacaoRepo.GetAllAsync();
        var naoLidas = todas.Where(n => n.UsuarioId == usuarioId && !n.Lida).ToList();
        foreach (var n in naoLidas)
        {
            n.Lida = true;
            await notificacaoRepo.UpdateAsync(n);
        }
    }
}
