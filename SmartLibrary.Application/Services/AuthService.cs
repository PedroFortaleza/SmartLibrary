using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Auth;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Application.Interfaces.Services;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Application.Services;

public class AuthService(IUsuarioRepository usuarioRepo, IConfiguration config) : IAuthService
{
    public async Task<TokenResponseDto> LoginAsync(LoginDto dto)
    {
        var usuario = await usuarioRepo.GetByEmailAsync(dto.Email)
            ?? throw new BusinessException("Email ou senha inválidos.");

        if (!usuario.Ativo)
            throw new BusinessException("Usuário inativo. Entre em contato com o administrador.");

        if (!BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
            throw new BusinessException("Email ou senha inválidos.");

        usuario.UltimoLogin = DateTime.UtcNow;
        await usuarioRepo.UpdateAsync(usuario);

        return GerarToken(usuario);
    }

    public async Task<TokenResponseDto> RegistroAsync(RegisterDto dto)
    {
        if (await usuarioRepo.GetByEmailAsync(dto.Email) != null)
            throw new BusinessException("E-mail já cadastrado.");

        if (!Enum.TryParse<TurnoAluno>(dto.Turno, true, out var turno))
            throw new BusinessException("Turno inválido. Use: Manha, Tarde ou Noite.");

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            Role = RoleUsuario.Aluno,
            Ativo = true,
            CriadoEm = DateTime.UtcNow,
            Aluno = new Aluno
            {
                Matricula = dto.Matricula,
                Curso = dto.Curso,
                Turno = turno
            }
        };

        await usuarioRepo.AddAsync(usuario);
        return GerarToken(usuario);
    }

    private TokenResponseDto GerarToken(Usuario usuario)
    {
        var key = config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key não configurada.");
        var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddHours(int.TryParse(config["Jwt:ExpiresInHours"], out var h) ? h : 8);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Role, usuario.Role.ToString()),
            new Claim("uid", usuario.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        return new TokenResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expires,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Role = usuario.Role.ToString()
        };
    }
}
