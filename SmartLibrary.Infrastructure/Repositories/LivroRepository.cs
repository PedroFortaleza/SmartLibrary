using Microsoft.EntityFrameworkCore;
using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Livros;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;
using SmartLibrary.Infrastructure.Data;

namespace SmartLibrary.Infrastructure.Repositories;

public class LivroRepository(SmartLibraryDbContext context) : BaseRepository<Livro>(context), ILivroRepository
{
    public async Task<PagedResult<LivroListDto>> GetPagedAsync(LivroFiltroDto filtro)
    {
        var query = Context.Livros
            .Include(l => l.LivroAutores).ThenInclude(la => la.Autor)
            .Include(l => l.LiveCategorias).ThenInclude(lc => lc.Categoria)
            .Include(l => l.Exemplares)
            .Include(l => l.Avaliacoes)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro.Search))
        {
            var s = filtro.Search.ToLower();
            query = query.Where(l =>
                l.Titulo.ToLower().Contains(s) ||
                l.ISBN.Contains(s) ||
                l.LivroAutores.Any(la => la.Autor.Nome.ToLower().Contains(s)));
        }

        if (filtro.CategoriaId.HasValue)
            query = query.Where(l => l.LiveCategorias.Any(lc => lc.CategoriaId == filtro.CategoriaId));

        if (filtro.AutorId.HasValue)
            query = query.Where(l => l.LivroAutores.Any(la => la.AutorId == filtro.AutorId));

        var total = await query.CountAsync();
        var livros = await query
            .OrderBy(l => l.Titulo)
            .Skip((filtro.Page - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .ToListAsync();

        return new PagedResult<LivroListDto>
        {
            TotalItems = total,
            Page = filtro.Page,
            PageSize = filtro.PageSize,
            Data = livros.Select(l => new LivroListDto
            {
                Id = l.Id,
                ISBN = l.ISBN,
                Titulo = l.Titulo,
                SubTitulo = l.SubTitulo,
                Editora = l.Editora,
                AnoPublicacao = l.AnoPublicacao,
                CapaUrl = l.CapaUrl,
                Autores = l.LivroAutores.OrderBy(la => la.Ordem).Select(la => la.Autor?.Nome ?? "").ToList(),
                Categorias = l.LiveCategorias.Select(lc => lc.Categoria?.Nome ?? "").ToList(),
                TotalExemplares = l.Exemplares.Count(e => e.Ativo),
                ExemplaresDisponiveis = l.Exemplares.Count(e => e.Ativo && e.Estado == EstadoExemplar.Disponivel),
                MediaAvaliacao = l.Avaliacoes.Any(a => a.Aprovada)
                    ? l.Avaliacoes.Where(a => a.Aprovada).Average(a => a.Nota)
                    : null
            })
        };
    }

    public async Task<Livro?> GetByIsbnAsync(string isbn)
        => await Context.Livros.FirstOrDefaultAsync(l => l.ISBN == isbn);

    public async Task<Livro?> GetWithDetailsAsync(int id)
        => await Context.Livros
            .Include(l => l.LivroAutores).ThenInclude(la => la.Autor)
            .Include(l => l.LiveCategorias).ThenInclude(lc => lc.Categoria)
            .Include(l => l.Exemplares)
            .Include(l => l.Avaliacoes).ThenInclude(a => a.Aluno).ThenInclude(a => a.Usuario)
            .FirstOrDefaultAsync(l => l.Id == id);

    public async Task<List<Exemplar>> GetExemplaresDisponiveisAsync(int livroId)
        => await Context.Exemplares
            .Where(e => e.LivroId == livroId && e.Ativo && e.Estado == EstadoExemplar.Disponivel)
            .ToListAsync();

    public async Task<bool> HasEmprestimosAtivosAsync(int livroId)
        => await Context.Emprestimos
            .AnyAsync(e => e.Exemplar.LivroId == livroId &&
                           e.Status != StatusEmprestimo.Devolvido);
}
