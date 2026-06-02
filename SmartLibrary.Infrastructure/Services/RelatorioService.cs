using Microsoft.EntityFrameworkCore;
using SmartLibrary.Application.DTOs.Relatorios;
using SmartLibrary.Application.Interfaces.Services;
using SmartLibrary.Domain.Enums;
using SmartLibrary.Infrastructure.Data;

namespace SmartLibrary.Infrastructure.Services;

public class RelatorioService(SmartLibraryDbContext context) : IRelatorioService
{
    public async Task<RelatorioAcervoDto> GetAcervoAsync()
    {
        var totalLivros = await context.Livros.CountAsync();
        var exemplares = await context.Exemplares.Where(e => e.Ativo).ToListAsync();

        var porCategoria = await context.Categorias
            .Select(c => new CategoriaAcervoDto
            {
                Categoria = c.Nome,
                TotalLivros = c.LiveCategorias.Count,
                TotalExemplares = c.LiveCategorias.SelectMany(lc => lc.Livro.Exemplares).Count(e => e.Ativo)
            }).ToListAsync();

        return new RelatorioAcervoDto
        {
            TotalLivros = totalLivros,
            TotalExemplares = exemplares.Count,
            ExemplaresDisponiveis = exemplares.Count(e => e.Estado == EstadoExemplar.Disponivel),
            ExemplaresEmprestados = exemplares.Count(e => e.Estado == EstadoExemplar.Emprestado),
            ExemplaresReservados = exemplares.Count(e => e.Estado == EstadoExemplar.Reservado),
            TotalEmprestimosAtivos = await context.Emprestimos
                .CountAsync(e => e.Status == StatusEmprestimo.Ativo || e.Status == StatusEmprestimo.Renovado),
            PorCategoria = porCategoria
        };
    }

    public async Task<RelatorioMultasDto> GetMultasAsync()
    {
        var multas = await context.Multas.ToListAsync();
        return new RelatorioMultasDto
        {
            TotalMultas = multas.Count,
            ValorTotal = multas.Sum(m => m.ValorTotal),
            Pendentes = multas.Count(m => m.Status == StatusMulta.Pendente),
            ValorPendente = multas.Where(m => m.Status == StatusMulta.Pendente).Sum(m => m.ValorTotal),
            Pagas = multas.Count(m => m.Status == StatusMulta.Pago),
            ValorPago = multas.Where(m => m.Status == StatusMulta.Pago).Sum(m => m.ValorTotal),
            Isentas = multas.Count(m => m.Status == StatusMulta.Isento)
        };
    }
}
