using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Avaliacoes;
using SmartLibrary.Application.Interfaces.Services;

namespace SmartLibrary.API.Controllers;

[ApiController]
[Route("api/avaliacoes")]
[Authorize]
public class AvaliacoesController(IAvaliacaoService avaliacaoService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Aluno")]
    public async Task<IActionResult> Create([FromBody] CreateAvaliacaoDto dto)
    {
        var avaliacao = await avaliacaoService.CreateAsync(dto, GetUsuarioId());
        return CreatedAtAction(nameof(Create), ApiResponse<AvaliacaoDto>.Success(avaliacao, "Avaliação enviada e aguardando aprovação."));
    }

    [HttpGet("pendentes")]
    [Authorize(Roles = "Bibliotecario,Administrador")]
    public async Task<IActionResult> GetPendentes()
    {
        var pendentes = await avaliacaoService.GetPendentesAsync();
        return Ok(ApiResponse<List<AvaliacaoDto>>.Success(pendentes));
    }

    [HttpPut("{id:int}/aprovar")]
    [Authorize(Roles = "Bibliotecario,Administrador")]
    public async Task<IActionResult> Aprovar(int id)
    {
        var avaliacao = await avaliacaoService.AprovarAsync(id);
        return Ok(ApiResponse<AvaliacaoDto>.Success(avaliacao, "Avaliação aprovada com sucesso."));
    }

    private int GetUsuarioId() => int.Parse(User.FindFirst("uid")?.Value ?? "0");
}
