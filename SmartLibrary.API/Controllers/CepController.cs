using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.External;
using SmartLibrary.Application.Interfaces.External;

namespace SmartLibrary.API.Controllers;

[ApiController]
[Route("api/cep")]
public class CepController(IViaCepService viaCepService) : ControllerBase
{
    [HttpGet("{cep}")]
    public async Task<IActionResult> GetByCep(string cep)
    {
        var resultado = await viaCepService.GetByCepAsync(cep);
        if (resultado == null)
            return NotFound(new { message = "CEP não encontrado." });
        return Ok(ApiResponse<ViaCepDto>.Success(resultado));
    }
}
