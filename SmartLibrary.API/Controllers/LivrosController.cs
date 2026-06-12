using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Livros;
using SmartLibrary.Application.Interfaces.Services;

namespace SmartLibrary.API.Controllers;

[ApiController]
[Route("api/livros")]
public class LivrosController(ILivroService livroService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] LivroFiltroDto filtro)
    {
        var result = await livroService.GetPagedAsync(filtro);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var livro = await livroService.GetByIdAsync(id);
        return Ok(ApiResponse<LivroDto>.Success(livro));
    }

    [HttpGet("{id:int}/disponibilidade")]
    public async Task<IActionResult> GetDisponibilidade(int id)
    {
        var exemplares = await livroService.GetDisponibilidadeAsync(id);
        return Ok(ApiResponse<object>.Success(exemplares));
    }

    [HttpGet("isbn/{isbn}")]
    public async Task<IActionResult> GetByIsbn(string isbn)
    {
        var livro = await livroService.GetByIsbnAsync(isbn);
        return Ok(ApiResponse<LivroDto>.Success(livro));
    }

    [HttpPost]
    [Authorize(Roles = "Bibliotecario,Administrador")]
    public async Task<IActionResult> Create([FromBody] CreateLivroDto dto)
    {
        var livro = await livroService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = livro.Id }, ApiResponse<LivroDto>.Success(livro, "Livro cadastrado com sucesso."));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Bibliotecario,Administrador")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLivroDto dto)
    {
        var livro = await livroService.UpdateAsync(id, dto);
        return Ok(ApiResponse<LivroDto>.Success(livro, "Livro atualizado com sucesso."));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        await livroService.DeleteAsync(id);
        return Ok(ApiResponse.Success("Livro excluído com sucesso."));
    }
}
