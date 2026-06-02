using SmartLibrary.Application.DTOs.Auth;

namespace SmartLibrary.Application.Interfaces.Services;

public interface IAuthService
{
    Task<TokenResponseDto> LoginAsync(LoginDto dto);
    Task<TokenResponseDto> RegistroAsync(RegisterDto dto);
}
