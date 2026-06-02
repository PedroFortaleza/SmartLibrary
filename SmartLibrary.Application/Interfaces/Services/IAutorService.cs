using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Autores;
using SmartLibrary.Application.DTOs.Livros;

namespace SmartLibrary.Application.Interfaces.Services;

public interface IAutorService
{
    Task<PagedResult<AutorDto>> GetPagedAsync(int page, int pageSize);
    Task<List<LivroListDto>> GetLivrosAsync(int autorId);
}
