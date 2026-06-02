using SmartLibrary.Application.DTOs.Parametros;

namespace SmartLibrary.Application.Interfaces.Services;

public interface IParametroService
{
    Task<List<ParametroDto>> GetAllAsync();
    Task<ParametroDto> UpdateAsync(string chave, UpdateParametroDto dto);
}
