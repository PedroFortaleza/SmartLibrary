using SmartLibrary.Application.DTOs.Reservas;

namespace SmartLibrary.Application.Interfaces.Services;

public interface IReservaService
{
    Task<List<ReservaDto>> GetAllAsync(int usuarioId, string role);
    Task<ReservaDto> CreateAsync(CreateReservaDto dto, int usuarioId);
    Task CancelarAsync(int id, int usuarioId, string role);
}
