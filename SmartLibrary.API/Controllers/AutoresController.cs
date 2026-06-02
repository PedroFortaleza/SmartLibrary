using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.Interfaces.Services;

namespace SmartLibrary.API.Controllers;

[ApiController]
[Route("api/autores")]
public class AutoresController(IAutorService autorService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await autorService.GetPagedAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:int}/livros")]
    public async Task<IActionResult> GetLivros(int id)
    {
        var livros = await autorService.GetLivrosAsync(id);
        return Ok(livros);
    }
}
