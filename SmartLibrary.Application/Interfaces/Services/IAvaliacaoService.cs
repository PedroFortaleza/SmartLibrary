using SmartLibrary.Application.DTOs.Avaliacoes;

namespace SmartLibrary.Application.Interfaces.Services;

public interface IAvaliacaoService
{
    Task<AvaliacaoDto> CreateAsync(CreateAvaliacaoDto dto, int usuarioId);
    Task<AvaliacaoDto> AprovarAsync(int id);
    Task<List<AvaliacaoDto>> GetPendentesAsync();
}
