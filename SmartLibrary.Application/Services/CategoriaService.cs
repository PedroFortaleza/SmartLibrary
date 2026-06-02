using SmartLibrary.Application.DTOs.Categorias;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Application.Interfaces.Services;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Application.Services;

public class CategoriaService(IBaseRepository<Categoria> categoriaRepo) : ICategoriaService
{
    public async Task<List<CategoriaDto>> GetAllAsync()
    {
        var cats = await categoriaRepo.GetAllAsync();
        return cats.Select(c => new CategoriaDto { Id = c.Id, Nome = c.Nome, Descricao = c.Descricao }).ToList();
    }
}
