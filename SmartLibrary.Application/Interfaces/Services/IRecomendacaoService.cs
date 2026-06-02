using SmartLibrary.Application.DTOs.Recomendacoes;

namespace SmartLibrary.Application.Interfaces.Services;

public interface IRecomendacaoService
{
    Task<List<RecomendacaoDto>> GetByUsuarioAsync(int usuarioId);
}
