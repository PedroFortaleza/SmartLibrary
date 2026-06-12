using SmartLibrary.Application.DTOs.Exemplares;

namespace SmartLibrary.Application.Interfaces.Services;

public interface IExemplarService
{
    Task<ExemplarDto> CreateAsync(CreateExemplarDto dto);
    Task<ExemplarDto> GetByIdAsync(int id);
    Task<ExemplarDto> UpdateEstadoAsync(int id, string novoEstado);
    Task DeleteAsync(int id);
}
