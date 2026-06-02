using SmartLibrary.Application.DTOs.Relatorios;

namespace SmartLibrary.Application.Interfaces.Services;

public interface IRelatorioService
{
    Task<RelatorioAcervoDto> GetAcervoAsync();
    Task<RelatorioMultasDto> GetMultasAsync();
}
