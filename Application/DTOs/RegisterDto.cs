using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public record RegisterDto(string Username, string Email, string Password);
}