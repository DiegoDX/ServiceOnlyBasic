using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Security.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, string connectionString, bool isTest)
        {
            // Register infrastructure services here
            services.AddDbContext<AppDbContext>(options =>
            {
                if (isTest)
                {
                    options.UseSqlite("DataSource=:memory:");
                }
                else
                {
                    options.UseSqlServer(configuration.GetConnectionString(connectionString));
                }
            });

            //services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(configuration.GetConnectionString(connectionString)));



            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();


            return services;
        }
    }
}
