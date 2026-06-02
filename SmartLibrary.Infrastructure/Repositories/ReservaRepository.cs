using Microsoft.EntityFrameworkCore;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;
using SmartLibrary.Infrastructure.Data;

namespace SmartLibrary.Infrastructure.Repositories;

public class ReservaRepository(SmartLibraryDbContext context) : BaseRepository<Reserva>(context), IReservaRepository
{
    public async Task<List<Reserva>> GetPendentesPorLivroAsync(int livroId)
        => await Context.Reservas
            .Where(r => r.LivroId == livroId && (r.Status == StatusReserva.Pendente || r.Status == StatusReserva.Notificado))
            .OrderBy(r => r.DataReserva)
            .ToListAsync();

    public async Task<List<Reserva>> GetByAlunoAsync(int alunoId)
        => await Context.Reservas
            .Include(r => r.Livro)
            .Include(r => r.Aluno).ThenInclude(a => a.Usuario)
            .Where(r => r.AlunoId == alunoId)
            .OrderByDescending(r => r.DataReserva)
            .ToListAsync();

    public async Task<List<Reserva>> GetAllWithDetailsAsync()
        => await Context.Reservas
            .Include(r => r.Livro)
            .Include(r => r.Aluno).ThenInclude(a => a.Usuario)
            .OrderByDescending(r => r.DataReserva)
            .ToListAsync();

    public async Task<bool> ExisteReservaPendenteAsync(int livroId, int alunoId)
        => await Context.Reservas.AnyAsync(r =>
            r.LivroId == livroId &&
            r.AlunoId == alunoId &&
            (r.Status == StatusReserva.Pendente || r.Status == StatusReserva.Notificado));
}
