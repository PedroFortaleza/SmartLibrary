using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Alunos;
using SmartLibrary.Application.Interfaces.External;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Application.Interfaces.Services;
using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Application.Services;

public class AlunoService(
    IUsuarioRepository usuarioRepo,
    IViaCepService viaCepService) : IAlunoService
{
    public async Task<AlunoPerfilDto> GetPerfilAsync(int usuarioId)
    {
        var aluno = await usuarioRepo.GetAlunoByUsuarioIdAsync(usuarioId)
            ?? throw new NotFoundException("Perfil de aluno não encontrado.");

        return MapToDto(aluno);
    }

    public async Task<AlunoPerfilDto> UpdatePerfilAsync(int usuarioId, UpdateAlunoPerfilDto dto)
    {
        var aluno = await usuarioRepo.GetAlunoByUsuarioIdAsync(usuarioId)
            ?? throw new NotFoundException("Perfil de aluno não encontrado.");

        if (!string.IsNullOrWhiteSpace(dto.Curso))
            aluno.Curso = dto.Curso;

        if (!string.IsNullOrWhiteSpace(dto.Turno))
        {
            if (!Enum.TryParse<TurnoAluno>(dto.Turno, true, out var turno))
                throw new BusinessException("Turno inválido. Use: Manha, Tarde ou Noite.");
            aluno.Turno = turno;
        }

        if (!string.IsNullOrWhiteSpace(dto.Cep))
        {
            var endereco = await viaCepService.GetByCepAsync(dto.Cep);
            if (endereco != null)
            {
                aluno.Cep = dto.Cep;
                aluno.Logradouro = endereco.Logradouro;
                aluno.Cidade = endereco.Localidade;
                aluno.UF = endereco.Uf;
            }
            else
            {
                aluno.Cep = dto.Cep;
            }
        }

        if (dto.Telefone != null)
            aluno.Telefone = dto.Telefone;

        if (dto.DataNascimento.HasValue)
            aluno.DataNascimento = dto.DataNascimento;

        await usuarioRepo.UpdateAsync(aluno.Usuario);
        return MapToDto(aluno);
    }

    public async Task<AlunoPerfilDto?> BuscarPorEmailAsync(string email)
    {
        var aluno = await usuarioRepo.GetAlunoByEmailAsync(email);
        return aluno == null ? null : MapToDto(aluno);
    }

    private static AlunoPerfilDto MapToDto(Domain.Entities.Aluno a) => new()
    {
        Id = a.Id,
        UsuarioId = a.UsuarioId,
        Nome = a.Usuario?.Nome ?? "",
        Email = a.Usuario?.Email ?? "",
        Matricula = a.Matricula,
        Curso = a.Curso,
        Turno = a.Turno.ToString(),
        Cep = a.Cep,
        Logradouro = a.Logradouro,
        Cidade = a.Cidade,
        UF = a.UF,
        DataNascimento = a.DataNascimento,
        Telefone = a.Telefone
    };
}
