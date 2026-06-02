using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Multas;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Application.Interfaces.Services;
using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Application.Services;

public class MultaService(
    IMultaRepository multaRepo,
    IUsuarioRepository usuarioRepo) : IMultaService
{
    public async Task<List<MultaDto>> GetAllAsync(int usuarioId, string role)
    {
        if (role == "Aluno")
        {
            var aluno = await usuarioRepo.GetAlunoByUsuarioIdAsync(usuarioId)
                ?? throw new NotFoundException("Perfil de aluno não encontrado.");
            var minhas = await multaRepo.GetPendentesPorAlunoAsync(aluno.Id);
            return minhas.Select(MapToDto).ToList();
        }

        var todas = await multaRepo.GetAllWithDetailsAsync();
        return todas.Select(MapToDto).ToList();
    }

    public async Task<MultaDto> PagarAsync(int id, PagarMultaDto dto)
    {
        var multa = await multaRepo.GetByIdAsync(id)
            ?? throw new NotFoundException("Multa não encontrada.");

        if (multa.Status != StatusMulta.Pendente)
            throw new BusinessException("Apenas multas pendentes podem ser pagas.");

        if (!Enum.TryParse<FormaPagamento>(dto.FormaPagamento, true, out var forma))
            throw new BusinessException("Forma de pagamento inválida. Use: Dinheiro, PIX ou Cartao.");

        multa.Status = StatusMulta.Pago;
        multa.DataPagamento = DateTime.UtcNow;
        multa.FormaPagamento = forma;

        await multaRepo.UpdateAsync(multa);
        return MapToDto(multa);
    }

    public async Task<MultaDto> IsentarAsync(int id)
    {
        var multa = await multaRepo.GetByIdAsync(id)
            ?? throw new NotFoundException("Multa não encontrada.");

        if (multa.Status != StatusMulta.Pendente)
            throw new BusinessException("Apenas multas pendentes podem ser isentadas.");

        multa.Status = StatusMulta.Isento;
        multa.DataPagamento = DateTime.UtcNow;

        await multaRepo.UpdateAsync(multa);
        return MapToDto(multa);
    }

    private static MultaDto MapToDto(Domain.Entities.Multa m) => new()
    {
        Id = m.Id,
        EmprestimoId = m.EmprestimoId,
        ValorDiario = m.ValorDiario,
        DiasAtraso = m.DiasAtraso,
        ValorTotal = m.ValorTotal,
        Status = m.Status.ToString(),
        DataPagamento = m.DataPagamento,
        FormaPagamento = m.FormaPagamento?.ToString()
    };
}
