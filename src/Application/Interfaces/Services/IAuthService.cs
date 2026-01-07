using Application.Common;
using Application.DTOs;

namespace Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto dto, CancellationToken cancellationToken);
        Task<AuthResponse> LoginAsync(LoginDto dto, CancellationToken cancellationToken);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
        Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken);
        Task<UserDto> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken);
    }
}