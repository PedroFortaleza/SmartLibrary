using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Emprestimos;
using SmartLibrary.Application.Interfaces.Services;

namespace SmartLibrary.API.Controllers;

[ApiController]
[Route("api/emprestimos")]
[Authorize]
public class EmprestimosController(IEmprestimoService emprestimoService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await emprestimoService.GetAllAsync(GetUsuarioId(), GetRole());
        return Ok(ApiResponse<List<EmprestimoDto>>.Success(result));
    }

    [HttpPost]
    [Authorize(Roles = "Bibliotecario,Administrador")]
    public async Task<IActionResult> Create([FromBody] CreateEmprestimoDto dto)
    {
        var emprestimo = await emprestimoService.CreateAsync(dto, GetUsuarioId());
        return CreatedAtAction(nameof(GetAll), ApiResponse<EmprestimoDto>.Success(emprestimo, "Empréstimo realizado com sucesso."));
    }

    [HttpPut("{id:int}/devolver")]
    [Authorize(Roles = "Bibliotecario,Administrador")]
    public async Task<IActionResult> Devolver(int id)
    {
        var emprestimo = await emprestimoService.DevolverAsync(id, GetUsuarioId());
        return Ok(ApiResponse<EmprestimoDto>.Success(emprestimo, "Devolução registrada com sucesso."));
    }

    [HttpPost("{id:int}/renovar")]
    public async Task<IActionResult> Renovar(int id)
    {
        var emprestimo = await emprestimoService.RenovarAsync(id, GetUsuarioId());
        return Ok(ApiResponse<EmprestimoDto>.Success(emprestimo, "Empréstimo renovado com sucesso."));
    }

    private int GetUsuarioId() => int.Parse(User.FindFirst("uid")?.Value ?? "0");
    private string GetRole() => User.FindFirst(ClaimTypes.Role)?.Value ?? "";
}
