using SmartLibrary.Application.DTOs.Multas;

namespace SmartLibrary.Application.Interfaces.Services;

public interface IMultaService
{
    Task<List<MultaDto>> GetAllAsync(int usuarioId, string role);
    Task<MultaDto> PagarAsync(int id, PagarMultaDto dto);
    Task<MultaDto> IsentarAsync(int id);
}
