using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Parametros;
using SmartLibrary.Application.Interfaces.Services;

namespace SmartLibrary.API.Controllers;

[ApiController]
[Route("api/parametros")]
[Authorize(Roles = "Administrador")]
public class ParametrosController(IParametroService parametroService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await parametroService.GetAllAsync();
        return Ok(ApiResponse<List<ParametroDto>>.Success(result));
    }

    [HttpPut("{chave}")]
    public async Task<IActionResult> Update(string chave, [FromBody] UpdateParametroDto dto)
    {
        var result = await parametroService.UpdateAsync(chave, dto);
        return Ok(ApiResponse<ParametroDto>.Success(result, "Parâmetro atualizado com sucesso."));
    }
}
