using SmartLibrary.Application.DTOs.External;

namespace SmartLibrary.Application.Interfaces.External;

public interface IViaCepService
{
    Task<ViaCepDto?> GetByCepAsync(string cep);
}
