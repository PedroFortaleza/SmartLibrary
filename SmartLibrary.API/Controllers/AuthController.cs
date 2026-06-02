using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.Common;
using SmartLibrary.Application.DTOs.Auth;
using SmartLibrary.Application.Interfaces.Services;

namespace SmartLibrary.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await authService.LoginAsync(dto);
        return Ok(ApiResponse<TokenResponseDto>.Success(token));
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registro([FromBody] RegisterDto dto)
    {
        var token = await authService.RegistroAsync(dto);
        return CreatedAtAction(nameof(Login), ApiResponse<TokenResponseDto>.Success(token, "Conta criada com sucesso."));
    }
}
