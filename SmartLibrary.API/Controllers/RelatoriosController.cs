using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Relatorios;
using SmartLibrary.Application.Interfaces.Services;

namespace SmartLibrary.API.Controllers;

[ApiController]
[Route("api/relatorios")]
[Authorize]
public class RelatoriosController(IRelatorioService relatorioService) : ControllerBase
{
    [HttpGet("acervo")]
    [Authorize(Roles = "Bibliotecario,Administrador")]
    public async Task<IActionResult> GetAcervo()
    {
        var result = await relatorioService.GetAcervoAsync();
        return Ok(ApiResponse<RelatorioAcervoDto>.Success(result));
    }

    [HttpGet("multas")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GetMultas()
    {
        var result = await relatorioService.GetMultasAsync();
        return Ok(ApiResponse<RelatorioMultasDto>.Success(result));
    }
}
