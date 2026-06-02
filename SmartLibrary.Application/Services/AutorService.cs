using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Autores;
using SmartLibrary.Application.DTOs.Livros;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Application.Interfaces.Services;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Application.Services;

public class AutorService(IBaseRepository<Autor> autorRepo, ILivroRepository livroRepo) : IAutorService
{
    public async Task<PagedResult<AutorDto>> GetPagedAsync(int page, int pageSize)
    {
        var todos = (await autorRepo.GetAllAsync()).ToList();
        var total = todos.Count;
        var items = todos.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(a => new AutorDto { Id = a.Id, Nome = a.Nome, Nacionalidade = a.Nacionalidade, Biografia = a.Biografia })
            .ToList();

        return new PagedResult<AutorDto> { Data = items, TotalItems = total, Page = page, PageSize = pageSize };
    }

    public async Task<List<LivroListDto>> GetLivrosAsync(int autorId)
    {
        var filtro = new LivroFiltroDto { AutorId = autorId, Page = 1, PageSize = 100 };
        var resultado = await livroRepo.GetPagedAsync(filtro);
        return resultado.Data.ToList();
    }
}
