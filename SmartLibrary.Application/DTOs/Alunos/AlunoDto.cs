using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Application.DTOs.Alunos;

public class AlunoPerfilDto
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Matricula { get; set; } = string.Empty;
    public string Curso { get; set; } = string.Empty;
    public string Turno { get; set; } = string.Empty;
    public string? Cep { get; set; }
    public string? Logradouro { get; set; }
    public string? Cidade { get; set; }
    public string? UF { get; set; }
    public DateOnly? DataNascimento { get; set; }
    public string? Telefone { get; set; }
}

public class UpdateAlunoPerfilDto
{
    [MaxLength(100)] public string? Curso { get; set; }
    public string? Turno { get; set; }
    [MaxLength(9)]  public string? Cep { get; set; }
    [MaxLength(20)] public string? Telefone { get; set; }
    public DateOnly? DataNascimento { get; set; }
}
