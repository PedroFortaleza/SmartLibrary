using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Exemplares;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Application.Interfaces.Services;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Application.Services;

public class ExemplarService(
    IExemplarRepository exemplarRepo,
    IBaseRepository<Livro> livroRepo) : IExemplarService
{
    public async Task<ExemplarDto> CreateAsync(CreateExemplarDto dto)
    {
        if (await livroRepo.GetByIdAsync(dto.LivroId) == null)
            throw new NotFoundException("Livro não encontrado.");

        if (await exemplarRepo.ExisteCodigoAsync(dto.Codigo))
            throw new BusinessException($"Já existe um exemplar com o código '{dto.Codigo}'.");

        if (!Enum.TryParse<TipoExemplar>(dto.Tipo, true, out var tipo))
            throw new BusinessException("Tipo inválido. Use: Fisico ou Digital.");

        var exemplar = new Exemplar
        {
            LivroId = dto.LivroId,
            Codigo = dto.Codigo,
            Localizacao = dto.Localizacao,
            Tipo = tipo,
            Estado = EstadoExemplar.Disponivel,
            Ativo = true,
            CriadoEm = DateTime.UtcNow
        };

        await exemplarRepo.AddAsync(exemplar);

        return MapToDto(exemplar);
    }

    public async Task<ExemplarDto> GetByIdAsync(int id)
    {
        var exemplar = await exemplarRepo.GetByIdAsync(id)
            ?? throw new NotFoundException("Exemplar não encontrado.");
        return MapToDto(exemplar);
    }

    public async Task<ExemplarDto> UpdateEstadoAsync(int id, string novoEstado)
    {
        var exemplar = await exemplarRepo.GetByIdAsync(id)
            ?? throw new NotFoundException("Exemplar não encontrado.");

        if (!Enum.TryParse<EstadoExemplar>(novoEstado, true, out var estado))
            throw new BusinessException("Estado inválido. Use: Disponivel, Emprestado, Reservado, Extraviado ou Danificado.");

        if (exemplar.Estado == EstadoExemplar.Emprestado && estado == EstadoExemplar.Disponivel)
            throw new BusinessException("Use o endpoint de devolução para liberar um exemplar emprestado.");

        exemplar.Estado = estado;
        await exemplarRepo.UpdateAsync(exemplar);
        return MapToDto(exemplar);
    }

    public async Task DeleteAsync(int id)
    {
        var exemplar = await exemplarRepo.GetByIdAsync(id)
            ?? throw new NotFoundException("Exemplar não encontrado.");

        if (exemplar.Estado == EstadoExemplar.Emprestado)
            throw new BusinessException("Não é possível excluir um exemplar que está emprestado.");

        await exemplarRepo.DeleteAsync(exemplar);
    }

    private static ExemplarDto MapToDto(Exemplar e) => new()
    {
        Id = e.Id,
        Codigo = e.Codigo,
        Localizacao = e.Localizacao,
        Tipo = e.Tipo.ToString(),
        Estado = e.Estado.ToString(),
        Ativo = e.Ativo
    };
}
