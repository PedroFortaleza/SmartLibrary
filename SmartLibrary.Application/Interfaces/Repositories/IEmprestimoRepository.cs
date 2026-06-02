using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Application.Interfaces.Repositories;

public interface IEmprestimoRepository : IBaseRepository<Emprestimo>
{
    Task<List<Emprestimo>> GetByAlunoAsync(int alunoId);
    Task<List<Emprestimo>> GetAllWithDetailsAsync();
    Task<Emprestimo?> GetWithDetailsAsync(int id);
    Task<int> CountAtivosDoAlunoAsync(int alunoId);
    Task<int> CountRenovacoesAsync(int emprestimoId);
}
