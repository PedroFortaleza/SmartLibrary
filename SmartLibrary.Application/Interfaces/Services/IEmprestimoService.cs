using SmartLibrary.Application.DTOs.Emprestimos;

namespace SmartLibrary.Application.Interfaces.Services;

public interface IEmprestimoService
{
    Task<List<EmprestimoDto>> GetAllAsync(int usuarioId, string role);
    Task<EmprestimoDto> GetByIdAsync(int id);
    Task<EmprestimoDto> CreateAsync(CreateEmprestimoDto dto, int bibliotecarioId);
    Task<EmprestimoDto> DevolverAsync(int id, int bibliotecarioId);
    Task<EmprestimoDto> RenovarAsync(int id, int usuarioId);
    Task<List<EmprestimoDto>> GetHistoricoByAlunoIdAsync(int alunoId);
}
