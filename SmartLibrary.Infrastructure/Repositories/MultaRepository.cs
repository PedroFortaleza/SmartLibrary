using Microsoft.EntityFrameworkCore;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;
using SmartLibrary.Infrastructure.Data;

namespace SmartLibrary.Infrastructure.Repositories;

public class MultaRepository(SmartLibraryDbContext context) : BaseRepository<Multa>(context), IMultaRepository
{
    public async Task<List<Multa>> GetPendentesPorAlunoAsync(int alunoId)
        => await Context.Multas
            .Include(m => m.Emprestimo)
            .Where(m => m.Emprestimo.AlunoId == alunoId)
            .ToListAsync();

    public async Task<List<Multa>> GetAllWithDetailsAsync()
        => await Context.Multas
            .Include(m => m.Emprestimo).ThenInclude(e => e.Aluno).ThenInclude(a => a.Usuario)
            .Include(m => m.Emprestimo).ThenInclude(e => e.Exemplar).ThenInclude(ex => ex.Livro)
            .ToListAsync();

    public async Task<bool> AlunoTemMultaPendenteAsync(int alunoId)
        => await Context.Multas
            .Include(m => m.Emprestimo)
            .AnyAsync(m => m.Emprestimo.AlunoId == alunoId && m.Status == StatusMulta.Pendente);
}
