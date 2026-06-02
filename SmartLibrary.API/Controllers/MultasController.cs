using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Multas;
using SmartLibrary.Application.Interfaces.Services;

namespace SmartLibrary.API.Controllers;

[ApiController]
[Route("api/multas")]
[Authorize]
public class MultasController(IMultaService multaService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await multaService.GetAllAsync(GetUsuarioId(), GetRole());
        return Ok(ApiResponse<List<MultaDto>>.Success(result));
    }

    [HttpPut("{id:int}/pagar")]
    [Authorize(Roles = "Bibliotecario,Administrador")]
    public async Task<IActionResult> Pagar(int id, [FromBody] PagarMultaDto dto)
    {
        var multa = await multaService.PagarAsync(id, dto);
        return Ok(ApiResponse<MultaDto>.Success(multa, "Multa paga com sucesso."));
    }

    [HttpPut("{id:int}/isentar")]
    [Authorize(Roles = "Bibliotecario,Administrador")]
    public async Task<IActionResult> Isentar(int id)
    {
        var multa = await multaService.IsentarAsync(id);
        return Ok(ApiResponse<MultaDto>.Success(multa, "Multa isenta com sucesso."));
    }

    private int GetUsuarioId() => int.Parse(User.FindFirst("uid")?.Value ?? "0");
    private string GetRole() => User.FindFirst(ClaimTypes.Role)?.Value ?? "";
}
