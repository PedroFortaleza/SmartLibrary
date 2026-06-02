using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Recomendacoes;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Application.Interfaces.Services;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Application.Services;

public class RecomendacaoService(
    IUsuarioRepository usuarioRepo,
    ILivroRepository livroRepo,
    IEmprestimoRepository emprestimoRepo,
    IBaseRepository<Recomendacao> recomendacaoRepo) : IRecomendacaoService
{
    public async Task<List<RecomendacaoDto>> GetByUsuarioAsync(int usuarioId)
    {
        var aluno = await usuarioRepo.GetAlunoByUsuarioIdAsync(usuarioId)
            ?? throw new NotFoundException("Perfil de aluno não encontrado.");

        var todas = await recomendacaoRepo.GetAllAsync();
        var minhas = todas.Where(r => r.AlunoId == aluno.Id).ToList();

        if (!minhas.Any())
            await GerarRecomendacoesAsync(aluno);

        minhas = (await recomendacaoRepo.GetAllAsync())
            .Where(r => r.AlunoId == aluno.Id)
            .OrderByDescending(r => r.Score)
            .ToList();

        return minhas.Select(r => new RecomendacaoDto
        {
            Id = r.Id,
            LivroId = r.LivroId,
            LivroTitulo = r.Livro?.Titulo ?? "",
            LivroCapaUrl = r.Livro?.CapaUrl,
            LivroEditora = r.Livro?.Editora,
            Autores = r.Livro?.LivroAutores.Select(la => la.Autor?.Nome ?? "").ToList() ?? [],
            Score = r.Score,
            Motivo = r.Motivo,
            Visualizada = r.Visualizada
        }).ToList();
    }

    private async Task GerarRecomendacoesAsync(Aluno aluno)
    {
        var emprestimos = await emprestimoRepo.GetByAlunoAsync(aluno.Id);
        var livrosEmprestados = emprestimos
            .Where(e => e.Status == StatusEmprestimo.Devolvido)
            .Select(e => e.Exemplar?.LivroId ?? 0)
            .Distinct()
            .ToHashSet();

        if (!livrosEmprestados.Any()) return;

        var categoriasFavoritas = emprestimos
            .Where(e => e.Exemplar?.Livro != null)
            .SelectMany(e => e.Exemplar!.Livro!.LiveCategorias.Select(lc => lc.CategoriaId))
            .GroupBy(id => id)
            .OrderByDescending(g => g.Count())
            .Take(3)
            .Select(g => g.Key)
            .ToHashSet();

        var filtro = new DTOs.Livros.LivroFiltroDto { Page = 1, PageSize = 50 };
        var pagina = await livroRepo.GetPagedAsync(filtro);

        var todas = await recomendacaoRepo.GetAllAsync();
        var jaRecomendados = todas.Where(r => r.AlunoId == aluno.Id).Select(r => r.LivroId).ToHashSet();

        foreach (var item in pagina.Data.Where(l => !livrosEmprestados.Contains(l.Id) && !jaRecomendados.Contains(l.Id)))
        {
            var livro = await livroRepo.GetWithDetailsAsync(item.Id);
            if (livro == null) continue;

            var categorias = livro.LiveCategorias.Select(lc => lc.CategoriaId).ToHashSet();
            var intersec = categorias.Intersect(categoriasFavoritas).Count();
            if (intersec == 0) continue;

            var score = Math.Min(100, intersec * 30m + (livro.Avaliacoes.Any(a => a.Aprovada)
                ? (decimal)livro.Avaliacoes.Where(a => a.Aprovada).Average(a => a.Nota) * 4
                : 0));

            await recomendacaoRepo.AddAsync(new Recomendacao
            {
                AlunoId = aluno.Id,
                LivroId = livro.Id,
                Score = score,
                Motivo = "Baseado nas categorias dos seus últimos empréstimos",
                Visualizada = false,
                CriadaEm = DateTime.UtcNow
            });
        }
    }
}
