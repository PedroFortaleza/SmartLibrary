using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Application.Interfaces.Repositories;

public interface IParametroRepository
{
    Task<string?> GetValorAsync(string chave);
    Task<int> GetIntAsync(string chave, int valorPadrao = 0);
    Task<decimal> GetDecimalAsync(string chave, decimal valorPadrao = 0);
    Task<List<ParametroSistema>> GetAllAsync();
    Task<ParametroSistema?> GetByChaveAsync(string chave);
    Task UpdateParametroAsync(ParametroSistema parametro);
}
