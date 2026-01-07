using Application.DTOs;
using Application.Interfaces.Services;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Core.Utils.Enums;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth) => _auth = auth;

        // ----------------------------
        // REGISTER
        // ----------------------------

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto, CancellationToken cancellationToken)
        {
            await _auth.RegisterAsync(dto, cancellationToken);

            return StatusCode(StatusCodes.Status201Created); ;
        }

        // ----------------------------
        // LOGIN
        // ----------------------------
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken cancellationToken)
        {
            var result = await _auth.LoginAsync(dto, cancellationToken);

            return Ok(result);
        }

        // ----------------------------
        // REFRESH TOKEN
        // ----------------------------
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Refresh([FromBody] string request, CancellationToken cancellationToken)
        {
            var result = await _auth.RefreshTokenAsync(request, cancellationToken);
            return Ok(result);
        }

        // ----------------------------
        // TEST AUTHENTICATED
        // ----------------------------
        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            return Ok(new
            {
                UserId = User.FindFirst("sub")?.Value,
                Email = User.FindFirst("email")?.Value,
                Role = User.FindFirst("role")?.Value
            });
        }

        [HttpGet("admin")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult AdminOnly()
        {
            return Ok("you are admin");
        }



    }
}