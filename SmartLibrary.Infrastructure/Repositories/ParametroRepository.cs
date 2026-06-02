using Microsoft.EntityFrameworkCore;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Infrastructure.Data;

namespace SmartLibrary.Infrastructure.Repositories;

public class ParametroRepository(SmartLibraryDbContext context) : IParametroRepository
{
    public async Task<string?> GetValorAsync(string chave)
        => (await context.ParametrosSistema.FirstOrDefaultAsync(p => p.Chave == chave))?.Valor;

    public async Task<int> GetIntAsync(string chave, int valorPadrao = 0)
    {
        var valor = await GetValorAsync(chave);
        return int.TryParse(valor, out var result) ? result : valorPadrao;
    }

    public async Task<decimal> GetDecimalAsync(string chave, decimal valorPadrao = 0)
    {
        var valor = await GetValorAsync(chave);
        return decimal.TryParse(valor, System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var result) ? result : valorPadrao;
    }

    public async Task<List<ParametroSistema>> GetAllAsync()
        => await context.ParametrosSistema.OrderBy(p => p.Chave).ToListAsync();

    public async Task<ParametroSistema?> GetByChaveAsync(string chave)
        => await context.ParametrosSistema.FirstOrDefaultAsync(p => p.Chave == chave);

    public async Task UpdateParametroAsync(ParametroSistema parametro)
    {
        context.ParametrosSistema.Update(parametro);
        await context.SaveChangesAsync();
    }
}
