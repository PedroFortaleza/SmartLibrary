using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Recomendacoes;
using SmartLibrary.Application.Interfaces.Services;

namespace SmartLibrary.API.Controllers;

[ApiController]
[Route("api/recomendacoes")]
[Authorize(Roles = "Aluno")]
public class RecomendacoesController(IRecomendacaoService recomendacaoService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await recomendacaoService.GetByUsuarioAsync(GetUsuarioId());
        return Ok(ApiResponse<List<RecomendacaoDto>>.Success(result));
    }

    private int GetUsuarioId() => int.Parse(User.FindFirst("uid")?.Value ?? "0");
}
