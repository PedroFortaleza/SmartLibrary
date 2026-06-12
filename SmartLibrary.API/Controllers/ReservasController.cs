using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Reservas;
using SmartLibrary.Application.Interfaces.Services;

namespace SmartLibrary.API.Controllers;

[ApiController]
[Route("api/reservas")]
[Authorize]
public class ReservasController(IReservaService reservaService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await reservaService.GetAllAsync(GetUsuarioId(), GetRole());
        return Ok(ApiResponse<List<ReservaDto>>.Success(result));
    }

    [HttpPost]
    [Authorize(Roles = "Aluno")]
    public async Task<IActionResult> Create([FromBody] CreateReservaDto dto)
    {
        var reserva = await reservaService.CreateAsync(dto, GetUsuarioId());
        return CreatedAtAction(nameof(GetAll), ApiResponse<ReservaDto>.Success(reserva, "Reserva realizada com sucesso."));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Cancelar(int id)
    {
        await reservaService.CancelarAsync(id, GetUsuarioId(), GetRole());
        return Ok(ApiResponse.Success("Reserva cancelada com sucesso."));
    }

    [HttpPut("{id:int}/notificar")]
    [Authorize(Roles = "Bibliotecario,Administrador")]
    public async Task<IActionResult> Notificar(int id)
    {
        await reservaService.NotificarAsync(id);
        return Ok(ApiResponse.Success("Aluno notificado com sucesso."));
    }

    [HttpPut("{id:int}/retirar")]
    [Authorize(Roles = "Bibliotecario,Administrador")]
    public async Task<IActionResult> ConfirmarRetirada(int id)
    {
        await reservaService.ConfirmarRetiradaAsync(id);
        return Ok(ApiResponse.Success("Retirada confirmada com sucesso."));
    }

    private int GetUsuarioId() => int.Parse(User.FindFirst("uid")?.Value ?? "0");
    private string GetRole() => User.FindFirst(ClaimTypes.Role)?.Value ?? "";
}
