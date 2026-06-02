using System.Text.Json;
using SmartLibrary.Application.DTOs.External;
using SmartLibrary.Application.Interfaces.External;

namespace SmartLibrary.Infrastructure.HttpClients;

public class ViaCepClient(HttpClient httpClient) : IViaCepService
{
    public async Task<ViaCepDto?> GetByCepAsync(string cep)
    {
        try
        {
            var cepLimpo = new string(cep.Where(char.IsDigit).ToArray());
            if (cepLimpo.Length != 8) return null;

            var response = await httpClient.GetAsync($"https://viacep.com.br/ws/{cepLimpo}/json/");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<ViaCepDto>(json);

            return dto?.Erro == true ? null : dto;
        }
        catch
        {
            return null;
        }
    }
}
