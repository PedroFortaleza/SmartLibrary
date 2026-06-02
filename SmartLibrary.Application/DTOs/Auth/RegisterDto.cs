using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Application.DTOs.Auth;

public class RegisterDto
{
    [Required] [MaxLength(150)] public string Nome { get; set; } = string.Empty;
    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] [MinLength(6)] public string Senha { get; set; } = string.Empty;
    [Required] [MaxLength(20)] public string Matricula { get; set; } = string.Empty;
    [Required] [MaxLength(100)] public string Curso { get; set; } = string.Empty;
    [Required] public string Turno { get; set; } = string.Empty;
}
