using SmartLibrary.Application.DTOs.Alunos;

namespace SmartLibrary.Application.Interfaces.Services;

public interface IAlunoService
{
    Task<AlunoPerfilDto> GetPerfilAsync(int usuarioId);
    Task<AlunoPerfilDto> UpdatePerfilAsync(int usuarioId, UpdateAlunoPerfilDto dto);
    Task<AlunoPerfilDto?> BuscarPorEmailAsync(string email);
}
