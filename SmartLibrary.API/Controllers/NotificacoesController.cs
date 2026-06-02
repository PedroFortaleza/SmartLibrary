using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Notificacoes;
using SmartLibrary.Application.Interfaces.Services;

namespace SmartLibrary.API.Controllers;

[ApiController]
[Route("api/notificacoes")]
[Authorize]
public class NotificacoesController(INotificacaoService notificacaoService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await notificacaoService.GetByUsuarioAsync(GetUsuarioId());
        return Ok(ApiResponse<List<NotificacaoDto>>.Success(result));
    }

    [HttpPut("{id:int}/ler")]
    public async Task<IActionResult> MarcarLida(int id)
    {
        await notificacaoService.MarcarLidaAsync(id, GetUsuarioId());
        return Ok(ApiResponse.Success("Notificação marcada como lida."));
    }

    [HttpPut("ler-todas")]
    public async Task<IActionResult> MarcarTodasLidas()
    {
        await notificacaoService.MarcarTodasLidasAsync(GetUsuarioId());
        return Ok(ApiResponse.Success("Todas as notificações marcadas como lidas."));
    }

    private int GetUsuarioId() => int.Parse(User.FindFirst("uid")?.Value ?? "0");
}
