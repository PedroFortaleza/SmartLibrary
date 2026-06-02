using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Alunos;
using SmartLibrary.Application.DTOs.Emprestimos;
using SmartLibrary.Application.Interfaces.Services;

namespace SmartLibrary.API.Controllers;

[ApiController]
[Route("api/alunos")]
[Authorize]
public class AlunosController(
    IEmprestimoService emprestimoService,
    IAlunoService alunoService) : ControllerBase
{
    [HttpGet("perfil")]
    [Authorize(Roles = "Aluno")]
    public async Task<IActionResult> GetPerfil()
    {
        var perfil = await alunoService.GetPerfilAsync(GetUsuarioId());
        return Ok(ApiResponse<AlunoPerfilDto>.Success(perfil));
    }

    [HttpPut("perfil")]
    [Authorize(Roles = "Aluno")]
    public async Task<IActionResult> UpdatePerfil([FromBody] UpdateAlunoPerfilDto dto)
    {
        var perfil = await alunoService.UpdatePerfilAsync(GetUsuarioId(), dto);
        return Ok(ApiResponse<AlunoPerfilDto>.Success(perfil, "Perfil atualizado com sucesso."));
    }

    [HttpGet("{alunoId:int}/historico")]
    [Authorize(Roles = "Bibliotecario,Administrador")]
    public async Task<IActionResult> GetHistorico(int alunoId)
    {
        var historico = await emprestimoService.GetHistoricoByAlunoIdAsync(alunoId);
        return Ok(ApiResponse<List<EmprestimoDto>>.Success(historico));
    }

    private int GetUsuarioId() => int.Parse(User.FindFirst("uid")?.Value ?? "0");
}
