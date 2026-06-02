using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Exemplares;
using SmartLibrary.Application.Interfaces.Services;

namespace SmartLibrary.API.Controllers;

[ApiController]
[Route("api/exemplares")]
[Authorize]
public class ExemplaresController(IExemplarService exemplarService) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var exemplar = await exemplarService.GetByIdAsync(id);
        return Ok(ApiResponse<ExemplarDto>.Success(exemplar));
    }

    [HttpPost]
    [Authorize(Roles = "Bibliotecario,Administrador")]
    public async Task<IActionResult> Create([FromBody] CreateExemplarDto dto)
    {
        var exemplar = await exemplarService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = exemplar.Id },
            ApiResponse<ExemplarDto>.Success(exemplar, "Exemplar cadastrado com sucesso."));
    }

    [HttpPut("{id:int}/estado")]
    [Authorize(Roles = "Bibliotecario,Administrador")]
    public async Task<IActionResult> UpdateEstado(int id, [FromBody] UpdateEstadoExemplarDto dto)
    {
        var exemplar = await exemplarService.UpdateEstadoAsync(id, dto.Estado);
        return Ok(ApiResponse<ExemplarDto>.Success(exemplar, "Estado atualizado com sucesso."));
    }
}
