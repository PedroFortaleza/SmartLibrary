using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Parametros;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Application.Interfaces.Services;

namespace SmartLibrary.Application.Services;

public class ParametroService(IParametroRepository parametroRepo) : IParametroService
{
    public async Task<List<ParametroDto>> GetAllAsync()
    {
        var todos = await parametroRepo.GetAllAsync();
        return todos.Select(p => new ParametroDto
        {
            Id = p.Id,
            Chave = p.Chave,
            Valor = p.Valor,
            Descricao = p.Descricao,
            AtualizadoEm = p.AtualizadoEm
        }).ToList();
    }

    public async Task<ParametroDto> UpdateAsync(string chave, UpdateParametroDto dto)
    {
        var parametro = await parametroRepo.GetByChaveAsync(chave)
            ?? throw new NotFoundException($"Parâmetro '{chave}' não encontrado.");

        parametro.Valor = dto.Valor;
        parametro.AtualizadoEm = DateTime.UtcNow;

        await parametroRepo.UpdateParametroAsync(parametro);

        return new ParametroDto
        {
            Id = parametro.Id,
            Chave = parametro.Chave,
            Valor = parametro.Valor,
            Descricao = parametro.Descricao,
            AtualizadoEm = parametro.AtualizadoEm
        };
    }
}
