using Microsoft.EntityFrameworkCore;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;
using SmartLibrary.Infrastructure.Data;

namespace SmartLibrary.Infrastructure.Repositories;

public class EmprestimoRepository(SmartLibraryDbContext context) : BaseRepository<Emprestimo>(context), IEmprestimoRepository
{
    private IQueryable<Emprestimo> QueryComDetalhes() =>
        Context.Emprestimos
            .Include(e => e.Exemplar).ThenInclude(ex => ex.Livro)
            .Include(e => e.Aluno).ThenInclude(a => a.Usuario)
            .Include(e => e.Bibliotecario)
            .Include(e => e.Renovacoes)
            .Include(e => e.Multa);

    public async Task<List<Emprestimo>> GetByAlunoAsync(int alunoId)
        => await QueryComDetalhes().Where(e => e.AlunoId == alunoId).ToListAsync();

    public async Task<List<Emprestimo>> GetAllWithDetailsAsync()
        => await QueryComDetalhes().OrderByDescending(e => e.DataEmprestimo).ToListAsync();

    public async Task<Emprestimo?> GetWithDetailsAsync(int id)
        => await QueryComDetalhes().FirstOrDefaultAsync(e => e.Id == id);

    public async Task<int> CountAtivosDoAlunoAsync(int alunoId)
        => await Context.Emprestimos.CountAsync(e =>
            e.AlunoId == alunoId &&
            (e.Status == StatusEmprestimo.Ativo || e.Status == StatusEmprestimo.Renovado));

    public async Task<int> CountRenovacoesAsync(int emprestimoId)
        => await Context.Renovacoes.CountAsync(r => r.EmprestimoId == emprestimoId);
}
