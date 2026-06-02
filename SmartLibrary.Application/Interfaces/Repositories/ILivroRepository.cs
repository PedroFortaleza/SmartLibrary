using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Livros;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Application.Interfaces.Repositories;

public interface ILivroRepository : IBaseRepository<Livro>
{
    Task<PagedResult<LivroListDto>> GetPagedAsync(LivroFiltroDto filtro);
    Task<Livro?> GetByIsbnAsync(string isbn);
    Task<Livro?> GetWithDetailsAsync(int id);
    Task<List<Exemplar>> GetExemplaresDisponiveisAsync(int livroId);
}
