using Application.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace WebAPI.DependencyInjection
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
                      
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtSettings = configuration
                        .GetSection("Jwt")
                        .Get<JwtSettings>();

                    // var jwtSettings = serviceProvider
                    //.GetRequiredService<IOptions<JwtSettings>>()
                    //.Value;

                    options.RequireHttpsMetadata = !env.IsDevelopment();
                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                        ClockSkew = TimeSpan.FromSeconds(30)
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly",
                    policy => policy.RequireRole("Admin"));
            });

            return services;
        }
    }
}
