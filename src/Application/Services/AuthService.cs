using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Core.Constants;
using Core.Entities;
using Core.Exceptions;
//using Microsoft.AspNetCore.Identity;
using BCrypt.Net;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        //private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtTokenGenerator _jwtGenerator;

        public AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IJwtTokenGenerator jwtGenerator)// IPasswordHasher<User> passwordHasher, 
        {
            _userRepository = userRepository;
        //    _passwordHasher = passwordHasher;
            _jwtGenerator = jwtGenerator;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task RegisterAsync(RegisterDto dto, CancellationToken cancellationToken)
        {
            var exists = await _userRepository.ExistsAsync(dto.Username, dto.Email);
            if (exists)
                throw new DomainException("Username or email already taken.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User(dto.Username, dto.Email, passwordHash);
           
            //user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            await _userRepository.AddAsync(user, cancellationToken);

            //var token = CreateToken(user);
            //return Result<AuthResultDto>.Success(new AuthResultDto(token.Token, token.ExpiresAt));
        }

        public async Task<AuthResponse> LoginAsync(LoginDto dto, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameOrEmailAsync(dto.Email, cancellationToken);
            if (user == null)
                throw new UnauthorizedException("Invalid credentials");
            //bool entre= false;
            //var password = dto.Password;
            ////var hash = BCrypt.Net.BCrypt.HashPassword(password);
            //var hash = user.PasswordHash.Trim();
            //var ses = "$2a$11$QA/JxXvGuKluhX91Gakftew6ehLpqqhjCl59ooDbK7lBC7kWW4kxi";
            //var ok = BCrypt.Net.BCrypt.Verify(password, ses);
            
            //if (ses == hash)
            //{
            //     entre= true;
            //}

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedException("Invalid credentials");

            //var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            //if (verify == PasswordVerificationResult.Failed)
            //    throw new UnauthorizedException("Invalid credentials");

            var accessToken = _jwtGenerator.GenerateAccessToken(user);
            var refreshToken = _jwtGenerator.GenerateRefreshToken();

            return new AuthResponse(accessToken, refreshToken, DateTime.UtcNow.AddMinutes(15));
        }



        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var storedToken = await _refreshTokenRepository.GetAsync(refreshToken, cancellationToken);

            if (storedToken == null)
                throw new UnauthorizedException("Invalid refresh token");
        
            if(storedToken.IsRevoked || storedToken.IsExpired)
                throw new UnauthorizedException("Refresh token has been revoked");

            storedToken.Revoke();
            var user = await _userRepository.GetByIdAsync(storedToken.UserId, cancellationToken);

            if (user == null)
                throw new NotFoundException( $"User {Constants.NotFound}");

            var accessToken = _jwtGenerator.GenerateAccessToken(user);  
            var newRefreshToken = _jwtGenerator.GenerateRefreshToken();

            await _refreshTokenRepository.AddAsync(new RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            }, cancellationToken);

            return new AuthResponse(accessToken, newRefreshToken, DateTime.UtcNow.AddMinutes(15));
        }

        public Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
