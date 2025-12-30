using System.Threading.RateLimiting;

namespace WebAPI.DependencyInjection
{
    public static class RateLimitingExtensions
    {
        public static IServiceCollection AddRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                //Policy por Ip (global)
                options.AddPolicy("IpPolicy", context =>
                {
                    var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: ip,
                        factory: _ => new FixedWindowRateLimiterOptions 
                        {
                            PermitLimit = 100,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        });
                });

                // 🔹 Policy para login (más estricta)
                options.AddPolicy("LoginPolicy", context =>
                {
                    var ip = context.Connection.RemoteIpAddress?.ToString()
                             ?? "unknown";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: $"login-{ip}",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        });
                });
            });

            return services;
        }
    }
}
