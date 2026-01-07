using Core.Entities;

namespace Application.Interfaces.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}