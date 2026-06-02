using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Exemplares;
using SmartLibrary.Application.DTOs.Livros;

namespace SmartLibrary.Application.Interfaces.Services;

public interface ILivroService
{
    Task<PagedResult<LivroListDto>> GetPagedAsync(LivroFiltroDto filtro);
    Task<LivroDto> GetByIdAsync(int id);
    Task<LivroDto> GetByIsbnAsync(string isbn);
    Task<List<ExemplarDto>> GetDisponibilidadeAsync(int id);
    Task<LivroDto> CreateAsync(CreateLivroDto dto);
    Task<LivroDto> UpdateAsync(int id, UpdateLivroDto dto);
}
