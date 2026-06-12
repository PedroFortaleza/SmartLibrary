using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Application.Interfaces.Repositories;

public interface IUsuarioRepository : IBaseRepository<Usuario>
{
    Task<Usuario?> GetByEmailAsync(string email);
    Task<Aluno?> GetAlunoByUsuarioIdAsync(int usuarioId);
    Task<Aluno?> GetAlunoByIdAsync(int alunoId);
    Task<Aluno?> GetAlunoByEmailAsync(string email);
}
