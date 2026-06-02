using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Avaliacoes;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Application.Interfaces.Services;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Application.Services;

public class AvaliacaoService(
    IBaseRepository<Avaliacao> avaliacaoRepo,
    IEmprestimoRepository emprestimoRepo,
    IUsuarioRepository usuarioRepo,
    IBaseRepository<Livro> livroRepo) : IAvaliacaoService
{
    public async Task<AvaliacaoDto> CreateAsync(CreateAvaliacaoDto dto, int usuarioId)
    {
        var aluno = await usuarioRepo.GetAlunoByUsuarioIdAsync(usuarioId)
            ?? throw new NotFoundException("Perfil de aluno não encontrado.");

        var livro = await livroRepo.GetByIdAsync(dto.LivroId)
            ?? throw new NotFoundException("Livro não encontrado.");

        var emprestimos = await emprestimoRepo.GetByAlunoAsync(aluno.Id);
        var jaPegou = emprestimos.Any(e =>
            e.Exemplar?.LivroId == dto.LivroId &&
            e.Status == StatusEmprestimo.Devolvido);

        if (!jaPegou)
            throw new BusinessException("Você só pode avaliar livros que já emprestou e devolveu.");

        var avaliacoes = await avaliacaoRepo.GetAllAsync();
        if (avaliacoes.Any(a => a.LivroId == dto.LivroId && a.AlunoId == aluno.Id))
            throw new BusinessException("Você já avaliou este livro.");

        var avaliacao = new Avaliacao
        {
            LivroId = dto.LivroId,
            AlunoId = aluno.Id,
            Nota = dto.Nota,
            Comentario = dto.Comentario,
            CriadaEm = DateTime.UtcNow,
            Aprovada = false
        };

        await avaliacaoRepo.AddAsync(avaliacao);

        return new AvaliacaoDto
        {
            Id = avaliacao.Id,
            LivroId = avaliacao.LivroId,
            LivroTitulo = livro.Titulo,
            AlunoId = avaliacao.AlunoId,
            Nota = avaliacao.Nota,
            Comentario = avaliacao.Comentario,
            CriadaEm = avaliacao.CriadaEm,
            Aprovada = avaliacao.Aprovada
        };
    }

    public async Task<AvaliacaoDto> AprovarAsync(int id)
    {
        var avaliacao = await avaliacaoRepo.GetByIdAsync(id)
            ?? throw new NotFoundException("Avaliação não encontrada.");

        avaliacao.Aprovada = true;
        await avaliacaoRepo.UpdateAsync(avaliacao);

        return new AvaliacaoDto
        {
            Id = avaliacao.Id,
            LivroId = avaliacao.LivroId,
            AlunoId = avaliacao.AlunoId,
            Nota = avaliacao.Nota,
            Comentario = avaliacao.Comentario,
            CriadaEm = avaliacao.CriadaEm,
            Aprovada = avaliacao.Aprovada
        };
    }

    public async Task<List<AvaliacaoDto>> GetPendentesAsync()
    {
        var todas = await avaliacaoRepo.GetAllAsync();
        return todas.Where(a => !a.Aprovada).Select(a => new AvaliacaoDto
        {
            Id = a.Id,
            LivroId = a.LivroId,
            AlunoId = a.AlunoId,
            Nota = a.Nota,
            Comentario = a.Comentario,
            CriadaEm = a.CriadaEm,
            Aprovada = a.Aprovada
        }).ToList();
    }
}
