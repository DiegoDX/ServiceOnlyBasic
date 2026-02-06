using Application.Interfaces.Services;
using Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;


namespace Application.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Services
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IAuthService, AuthService>();


            //Validators
            services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            return services;
        }
    }
}
