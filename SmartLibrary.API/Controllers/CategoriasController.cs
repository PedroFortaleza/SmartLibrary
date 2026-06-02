using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Categorias;
using SmartLibrary.Application.Interfaces.Services;

namespace SmartLibrary.API.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriasController(ICategoriaService categoriaService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await categoriaService.GetAllAsync();
        return Ok(ApiResponse<List<CategoriaDto>>.Success(result));
    }
}
