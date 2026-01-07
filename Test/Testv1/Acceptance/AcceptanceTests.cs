using Application.DTOs;
using Core.Entities;
using Core.Utils;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Test.Common;
using static Core.Utils.Enums;

public class AcceptanceTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;
    private readonly HttpClient _client;

    public AcceptanceTests(ApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCities_ShouldReturn200()
    {
        await SeedUserAsync();

        var token = await LoginAndGetToken();

        _client.DefaultRequestHeaders.Authorization =
           new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/cities");
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateCountry_ShouldFail_WhenNoToken()
    {
        var dto = new { name = "Bolivia" };

        var response = await _client.PostAsJsonAsync("/api/countries", dto);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateCountry_ShouldSucceed_WhenAuthenticated()
    {
        await SeedUserAsync();

        var token = await LoginAndGetToken();

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var dto = new { name = "Bolivia" };

        var response = await _client.PostAsJsonAsync("/api/countries", dto);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // -------------------------
    // Helpers
    // -------------------------

    private async Task<string> LoginAndGetToken()
    {
        var login = new
        {
            usernameOrEmail = "admin",
            password = "123456"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", login);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AuthResultDto>();

        return result!.Token;
    }

    private async Task SeedUserAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

        if (!ctx.Users.Any())
        {
            var user = new User
            {
                Username = "admin",
                Email = "admin@test.com",
                Role = nameof(UserRole.Admin)
            };

            user.PasswordHash = hasher.HashPassword(user, "123456");

            ctx.Users.Add(user);
            await ctx.SaveChangesAsync();
        }
    }
}
