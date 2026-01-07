using Application.Interfaces.Services;
using Application.Jwt;
using Core.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Security.Jwt
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSetting;

        public JwtTokenGenerator(IOptions<JwtSettings> jwtSetting)
        {
            _jwtSetting = jwtSetting.Value;
        }

        public string GenerateAccessToken(User user)
        {
            //additional attributes  about the user.
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Secret));

            var token = new JwtSecurityToken(
                claims: claims,
                issuer: _jwtSetting.Issuer,
                audience: _jwtSetting.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSetting.ExpMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
