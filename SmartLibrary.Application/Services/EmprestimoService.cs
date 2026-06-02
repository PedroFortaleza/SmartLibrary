using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Emprestimos;
using SmartLibrary.Application.DTOs.Multas;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Application.Interfaces.Services;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Application.Services;

public class EmprestimoService(
    IEmprestimoRepository emprestimoRepo,
    IMultaRepository multaRepo,
    IReservaRepository reservaRepo,
    IExemplarRepository exemplarRepo,
    IUsuarioRepository usuarioRepo,
    IParametroRepository parametroRepo) : IEmprestimoService
{
    public async Task<List<EmprestimoDto>> GetAllAsync(int usuarioId, string role)
    {
        List<Emprestimo> lista;
        if (role == "Aluno")
        {
            var aluno = await usuarioRepo.GetAlunoByUsuarioIdAsync(usuarioId)
                ?? throw new NotFoundException("Perfil de aluno não encontrado.");
            lista = await emprestimoRepo.GetByAlunoAsync(aluno.Id);
        }
        else
        {
            lista = await emprestimoRepo.GetAllWithDetailsAsync();
        }
        return lista.Select(MapToDto).ToList();
    }

    public async Task<EmprestimoDto> GetByIdAsync(int id)
    {
        var e = await emprestimoRepo.GetWithDetailsAsync(id)
            ?? throw new NotFoundException("Empréstimo não encontrado.");
        return MapToDto(e);
    }

    public async Task<EmprestimoDto> CreateAsync(CreateEmprestimoDto dto, int bibliotecarioId)
    {
        var exemplar = await exemplarRepo.GetByIdAsync(dto.ExemplarId)
            ?? throw new NotFoundException("Exemplar não encontrado.");

        if (exemplar.Estado != EstadoExemplar.Disponivel)
            throw new BusinessException("Exemplar não está disponível para empréstimo.");

        if (await multaRepo.AlunoTemMultaPendenteAsync(dto.AlunoId))
            throw new BusinessException("Aluno possui multa pendente e não pode realizar empréstimo.");

        var maxEmprestimos = await parametroRepo.GetIntAsync("MaxEmprestimosPorAluno", 3);
        var totalAtivos = await emprestimoRepo.CountAtivosDoAlunoAsync(dto.AlunoId);
        if (totalAtivos >= maxEmprestimos)
            throw new BusinessException($"Aluno atingiu o limite de {maxEmprestimos} empréstimos simultâneos.");

        var diasEmprestimo = await parametroRepo.GetIntAsync("DiasEmprestimo", 7);

        var emprestimo = new Emprestimo
        {
            ExemplarId = dto.ExemplarId,
            AlunoId = dto.AlunoId,
            BibliotecarioId = bibliotecarioId,
            DataEmprestimo = DateTime.UtcNow,
            DataPrevistaDevolucao = DateTime.UtcNow.AddDays(diasEmprestimo),
            Status = StatusEmprestimo.Ativo,
            Observacao = dto.Observacao
        };

        exemplar.Estado = EstadoExemplar.Emprestado;
        await exemplarRepo.UpdateAsync(exemplar);
        await emprestimoRepo.AddAsync(emprestimo);

        return await GetByIdAsync(emprestimo.Id);
    }

    public async Task<EmprestimoDto> DevolverAsync(int id, int bibliotecarioId)
    {
        var emprestimo = await emprestimoRepo.GetWithDetailsAsync(id)
            ?? throw new NotFoundException("Empréstimo não encontrado.");

        if (emprestimo.Status == StatusEmprestimo.Devolvido)
            throw new BusinessException("Este empréstimo já foi devolvido.");

        var agora = DateTime.UtcNow;
        emprestimo.DataDevolucao = agora;
        emprestimo.Status = StatusEmprestimo.Devolvido;

        if (agora > emprestimo.DataPrevistaDevolucao)
        {
            var diasAtraso = Math.Max(1, (int)(agora - emprestimo.DataPrevistaDevolucao).TotalDays);
            var valorDiario = await parametroRepo.GetDecimalAsync("ValorMultaDiaria", 0.50m);

            var multa = new Multa
            {
                EmprestimoId = emprestimo.Id,
                ValorDiario = valorDiario,
                DiasAtraso = diasAtraso,
                ValorTotal = diasAtraso * valorDiario,
                Status = StatusMulta.Pendente
            };
            await multaRepo.AddAsync(multa);
        }

        var exemplar = emprestimo.Exemplar;
        var proximaReserva = (await reservaRepo.GetPendentesPorLivroAsync(exemplar.LivroId)).FirstOrDefault();

        if (proximaReserva != null)
        {
            var horas = await parametroRepo.GetIntAsync("HorasParaRetiradaReserva", 48);
            proximaReserva.Status = StatusReserva.Notificado;
            proximaReserva.NotificadoEm = agora;
            proximaReserva.DataExpiracao = agora.AddHours(horas);
            await reservaRepo.UpdateAsync(proximaReserva);
            exemplar.Estado = EstadoExemplar.Reservado;
        }
        else
        {
            exemplar.Estado = EstadoExemplar.Disponivel;
        }

        await exemplarRepo.UpdateAsync(exemplar);
        await emprestimoRepo.UpdateAsync(emprestimo);

        return MapToDto(emprestimo);
    }

    public async Task<EmprestimoDto> RenovarAsync(int id, int usuarioId)
    {
        var emprestimo = await emprestimoRepo.GetWithDetailsAsync(id)
            ?? throw new NotFoundException("Empréstimo não encontrado.");

        if (emprestimo.Status != StatusEmprestimo.Ativo && emprestimo.Status != StatusEmprestimo.Renovado)
            throw new BusinessException("Apenas empréstimos ativos podem ser renovados.");

        var maxRenovacoes = await parametroRepo.GetIntAsync("MaxRenovacoes", 2);
        var totalRenovacoes = await emprestimoRepo.CountRenovacoesAsync(id);
        if (totalRenovacoes >= maxRenovacoes)
            throw new BusinessException($"Empréstimo atingiu o limite de {maxRenovacoes} renovações.");

        var reservasPendentes = await reservaRepo.GetPendentesPorLivroAsync(emprestimo.Exemplar.LivroId);
        if (reservasPendentes.Any())
            throw new BusinessException("Não é possível renovar: existe reserva pendente de outro aluno para este livro.");

        var diasEmprestimo = await parametroRepo.GetIntAsync("DiasEmprestimo", 7);
        var novaData = emprestimo.DataPrevistaDevolucao.AddDays(diasEmprestimo);

        emprestimo.Renovacoes.Add(new Renovacao
        {
            EmprestimoId = emprestimo.Id,
            DataRenovacao = DateTime.UtcNow,
            NovaDataPrevista = novaData,
            UsuarioId = usuarioId
        });

        emprestimo.DataPrevistaDevolucao = novaData;
        emprestimo.Status = StatusEmprestimo.Renovado;

        await emprestimoRepo.UpdateAsync(emprestimo);
        return MapToDto(emprestimo);
    }

    public async Task<List<EmprestimoDto>> GetHistoricoByAlunoIdAsync(int alunoId)
    {
        var lista = await emprestimoRepo.GetByAlunoAsync(alunoId);
        return lista.OrderByDescending(e => e.DataEmprestimo).Select(MapToDto).ToList();
    }

    private static EmprestimoDto MapToDto(Emprestimo e) => new()
    {
        Id = e.Id,
        ExemplarId = e.ExemplarId,
        ExemplarCodigo = e.Exemplar?.Codigo ?? "",
        LivroTitulo = e.Exemplar?.Livro?.Titulo ?? "",
        LivroId = e.Exemplar?.LivroId ?? 0,
        AlunoId = e.AlunoId,
        AlunoNome = e.Aluno?.Usuario?.Nome ?? "",
        BibliotecarioNome = e.Bibliotecario?.Nome ?? "",
        DataEmprestimo = e.DataEmprestimo,
        DataPrevistaDevolucao = e.DataPrevistaDevolucao,
        DataDevolucao = e.DataDevolucao,
        Status = e.Status.ToString(),
        Observacao = e.Observacao,
        TotalRenovacoes = e.Renovacoes?.Count ?? 0,
        Multa = e.Multa == null ? null : new MultaDto
        {
            Id = e.Multa.Id,
            EmprestimoId = e.Multa.EmprestimoId,
            ValorDiario = e.Multa.ValorDiario,
            DiasAtraso = e.Multa.DiasAtraso,
            ValorTotal = e.Multa.ValorTotal,
            Status = e.Multa.Status.ToString(),
            DataPagamento = e.Multa.DataPagamento,
            FormaPagamento = e.Multa.FormaPagamento?.ToString()
        }
    };
}
