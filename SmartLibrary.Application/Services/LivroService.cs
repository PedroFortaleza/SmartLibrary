using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Avaliacoes;
using SmartLibrary.Application.DTOs.Exemplares;
using SmartLibrary.Application.DTOs.Livros;
using SmartLibrary.Application.Interfaces.External;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Application.Interfaces.Services;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Application.Services;

public class LivroService(
    ILivroRepository livroRepo,
    IGoogleBooksService googleBooks,
    IOpenLibraryService openLibrary) : ILivroService
{
    public async Task<PagedResult<LivroListDto>> GetPagedAsync(LivroFiltroDto filtro)
        => await livroRepo.GetPagedAsync(filtro);

    public async Task<LivroDto> GetByIdAsync(int id)
    {
        var livro = await livroRepo.GetWithDetailsAsync(id)
            ?? throw new NotFoundException("Livro não encontrado.");
        return MapToDto(livro);
    }

    public async Task<LivroDto> GetByIsbnAsync(string isbn)
    {
        var livro = await livroRepo.GetByIsbnAsync(isbn);
        if (livro != null)
            return MapToDto(livro);

        var resultado = await googleBooks.GetByIsbnAsync(isbn)
                     ?? await openLibrary.GetByIsbnAsync(isbn);

        if (resultado == null)
            throw new NotFoundException($"Livro com ISBN '{isbn}' não encontrado.");

        return new LivroDto
        {
            ISBN = isbn,
            Titulo = resultado.Titulo ?? string.Empty,
            SubTitulo = resultado.SubTitulo,
            Editora = resultado.Editora,
            AnoPublicacao = resultado.AnoPublicacao,
            Idioma = resultado.Idioma,
            NumeroPaginas = resultado.NumeroPaginas,
            Sinopse = resultado.Descricao,
            CapaUrl = resultado.CapaUrl,
            Autores = resultado.Autores
        };
    }

    public async Task<List<ExemplarDto>> GetDisponibilidadeAsync(int id)
    {
        var exemplares = await livroRepo.GetExemplaresDisponiveisAsync(id);
        return exemplares.Select(e => new ExemplarDto
        {
            Id = e.Id,
            Codigo = e.Codigo,
            Localizacao = e.Localizacao,
            Tipo = e.Tipo.ToString(),
            Estado = e.Estado.ToString(),
            Ativo = e.Ativo
        }).ToList();
    }

    public async Task<LivroDto> CreateAsync(CreateLivroDto dto)
    {
        if (await livroRepo.GetByIsbnAsync(dto.ISBN) != null)
            throw new BusinessException($"Já existe um livro com o ISBN '{dto.ISBN}'.");

        var livro = new Livro
        {
            ISBN = dto.ISBN,
            Titulo = dto.Titulo,
            SubTitulo = dto.SubTitulo,
            Editora = dto.Editora,
            AnoPublicacao = dto.AnoPublicacao,
            Edicao = dto.Edicao,
            Idioma = dto.Idioma,
            NumeroPaginas = dto.NumeroPaginas,
            Sinopse = dto.Sinopse,
            CapaUrl = dto.CapaUrl,
            CriadoEm = DateTime.UtcNow,
            LivroAutores = dto.AutorIds.Select((id, i) => new LivroAutor { AutorId = id, Ordem = i + 1 }).ToList(),
            LiveCategorias = dto.CategoriaIds.Select(id => new LivroCategoria { CategoriaId = id }).ToList()
        };

        await livroRepo.AddAsync(livro);
        return await GetByIdAsync(livro.Id);
    }

    public async Task<LivroDto> UpdateAsync(int id, UpdateLivroDto dto)
    {
        var livro = await livroRepo.GetWithDetailsAsync(id)
            ?? throw new NotFoundException("Livro não encontrado.");

        var existente = await livroRepo.GetByIsbnAsync(dto.ISBN);
        if (existente != null && existente.Id != id)
            throw new BusinessException($"Já existe outro livro com o ISBN '{dto.ISBN}'.");

        livro.ISBN = dto.ISBN;
        livro.Titulo = dto.Titulo;
        livro.SubTitulo = dto.SubTitulo;
        livro.Editora = dto.Editora;
        livro.AnoPublicacao = dto.AnoPublicacao;
        livro.Edicao = dto.Edicao;
        livro.Idioma = dto.Idioma;
        livro.NumeroPaginas = dto.NumeroPaginas;
        livro.Sinopse = dto.Sinopse;
        livro.CapaUrl = dto.CapaUrl;

        livro.LivroAutores.Clear();
        foreach (var (autorId, i) in dto.AutorIds.Select((a, i) => (a, i)))
            livro.LivroAutores.Add(new LivroAutor { LivroId = id, AutorId = autorId, Ordem = i + 1 });

        livro.LiveCategorias.Clear();
        foreach (var catId in dto.CategoriaIds)
            livro.LiveCategorias.Add(new LivroCategoria { LivroId = id, CategoriaId = catId });

        await livroRepo.UpdateAsync(livro);
        return await GetByIdAsync(id);
    }

    private static LivroDto MapToDto(Livro livro) => new()
    {
        Id = livro.Id,
        ISBN = livro.ISBN,
        Titulo = livro.Titulo,
        SubTitulo = livro.SubTitulo,
        Editora = livro.Editora,
        AnoPublicacao = livro.AnoPublicacao,
        CapaUrl = livro.CapaUrl,
        Idioma = livro.Idioma,
        NumeroPaginas = livro.NumeroPaginas,
        Sinopse = livro.Sinopse,
        Edicao = livro.Edicao,
        CriadoEm = livro.CriadoEm,
        Autores = livro.LivroAutores.OrderBy(la => la.Ordem).Select(la => la.Autor?.Nome ?? "").ToList(),
        Categorias = livro.LiveCategorias.Select(lc => lc.Categoria?.Nome ?? "").ToList(),
        TotalExemplares = livro.Exemplares.Count(e => e.Ativo),
        ExemplaresDisponiveis = livro.Exemplares.Count(e => e.Ativo && e.Estado == EstadoExemplar.Disponivel),
        MediaAvaliacao = livro.Avaliacoes.Any(a => a.Aprovada)
            ? livro.Avaliacoes.Where(a => a.Aprovada).Average(a => a.Nota)
            : null,
        Exemplares = livro.Exemplares.Select(e => new ExemplarDto
        {
            Id = e.Id, Codigo = e.Codigo, Localizacao = e.Localizacao,
            Tipo = e.Tipo.ToString(), Estado = e.Estado.ToString(), Ativo = e.Ativo
        }).ToList(),
        Avaliacoes = livro.Avaliacoes.Where(a => a.Aprovada).Select(a => new AvaliacaoDto
        {
            Id = a.Id, LivroId = a.LivroId, AlunoId = a.AlunoId,
            AlunoNome = a.Aluno?.Usuario?.Nome ?? "",
            Nota = a.Nota, Comentario = a.Comentario, CriadaEm = a.CriadaEm, Aprovada = a.Aprovada
        }).ToList()
    };
}
