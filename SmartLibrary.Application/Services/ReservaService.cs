using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Reservas;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Application.Interfaces.Services;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Application.Services;

public class ReservaService(
    IReservaRepository reservaRepo,
    ILivroRepository livroRepo,
    IUsuarioRepository usuarioRepo,
    IParametroRepository parametroRepo) : IReservaService
{
    public async Task<List<ReservaDto>> GetAllAsync(int usuarioId, string role)
    {
        List<Reserva> lista;
        if (role == "Aluno")
        {
            var aluno = await usuarioRepo.GetAlunoByUsuarioIdAsync(usuarioId)
                ?? throw new NotFoundException("Perfil de aluno não encontrado.");
            lista = await reservaRepo.GetByAlunoAsync(aluno.Id);
        }
        else
        {
            lista = await reservaRepo.GetAllWithDetailsAsync();
        }
        return lista.Select(MapToDto).ToList();
    }

    public async Task<ReservaDto> CreateAsync(CreateReservaDto dto, int usuarioId)
    {
        var aluno = await usuarioRepo.GetAlunoByUsuarioIdAsync(usuarioId)
            ?? throw new NotFoundException("Perfil de aluno não encontrado.");

        var livro = await livroRepo.GetWithDetailsAsync(dto.LivroId)
            ?? throw new NotFoundException("Livro não encontrado.");

        var exemplares = await livroRepo.GetExemplaresDisponiveisAsync(dto.LivroId);
        if (exemplares.Any())
            throw new BusinessException("Há exemplares disponíveis. Faça o empréstimo diretamente com o bibliotecário.");

        if (await reservaRepo.ExisteReservaPendenteAsync(dto.LivroId, aluno.Id))
            throw new BusinessException("Você já possui uma reserva ativa para este livro.");

        var horasExpiracao = await parametroRepo.GetIntAsync("HorasParaRetiradaReserva", 48);

        var reserva = new Reserva
        {
            LivroId = dto.LivroId,
            AlunoId = aluno.Id,
            DataReserva = DateTime.UtcNow,
            DataExpiracao = DateTime.UtcNow.AddHours(horasExpiracao * 30),
            Status = StatusReserva.Pendente
        };

        await reservaRepo.AddAsync(reserva);

        var reservaCriada = (await reservaRepo.GetAllWithDetailsAsync())
            .FirstOrDefault(r => r.Id == reserva.Id) ?? reserva;

        return MapToDto(reservaCriada);
    }

    public async Task CancelarAsync(int id, int usuarioId, string role)
    {
        var reserva = await reservaRepo.GetByIdAsync(id)
            ?? throw new NotFoundException("Reserva não encontrada.");

        if (role == "Aluno")
        {
            var aluno = await usuarioRepo.GetAlunoByUsuarioIdAsync(usuarioId)
                ?? throw new NotFoundException("Perfil de aluno não encontrado.");
            if (reserva.AlunoId != aluno.Id)
                throw new BusinessException("Você não tem permissão para cancelar esta reserva.");
        }

        if (reserva.Status == StatusReserva.Retirado || reserva.Status == StatusReserva.Cancelado)
            throw new BusinessException("Esta reserva não pode ser cancelada.");

        reserva.Status = StatusReserva.Cancelado;
        await reservaRepo.UpdateAsync(reserva);
    }

    public async Task NotificarAsync(int id)
    {
        var reserva = await reservaRepo.GetByIdAsync(id)
            ?? throw new NotFoundException("Reserva não encontrada.");

        if (reserva.Status != StatusReserva.Pendente)
            throw new BusinessException("Apenas reservas com status Pendente podem ser notificadas.");

        var horasExpiracao = await parametroRepo.GetIntAsync("HorasParaRetiradaReserva", 48);

        reserva.Status = StatusReserva.Notificado;
        reserva.NotificadoEm = DateTime.UtcNow;
        reserva.DataExpiracao = DateTime.UtcNow.AddHours(horasExpiracao);
        await reservaRepo.UpdateAsync(reserva);
    }

    public async Task ConfirmarRetiradaAsync(int id)
    {
        var reserva = await reservaRepo.GetByIdAsync(id)
            ?? throw new NotFoundException("Reserva não encontrada.");

        if (reserva.Status != StatusReserva.Notificado)
            throw new BusinessException("Apenas reservas com status Notificado podem ter retirada confirmada.");

        reserva.Status = StatusReserva.Retirado;
        await reservaRepo.UpdateAsync(reserva);
    }

    private static ReservaDto MapToDto(Reserva r) => new()
    {
        Id = r.Id,
        LivroId = r.LivroId,
        LivroTitulo = r.Livro?.Titulo ?? "",
        LivroCapaUrl = r.Livro?.CapaUrl,
        AlunoId = r.AlunoId,
        AlunoNome = r.Aluno?.Usuario?.Nome ?? "",
        DataReserva = r.DataReserva,
        DataExpiracao = r.DataExpiracao,
        Status = r.Status.ToString(),
        NotificadoEm = r.NotificadoEm
    };
}
