using Application.DependencyInjection;
using Infrastructure.Data;
using Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using WebAPI.DependencyInjection;
using WebAPI.Middleware;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ---------------------------- Services ----------------------------

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Host.UseSerilog((ctx, lc) =>
            {
                lc.MinimumLevel.Information()
                  .Enrich.FromLogContext()
                  .WriteTo.Console()
                  .WriteTo.File(
                    path: "Logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                    );
            });

            // Application & Infrastructure
            bool isTest = builder.Environment.IsEnvironment("Testing");
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration, "DefaultConnection", isTest);
            builder.Services.AddJwtAuthentication(
                    builder.Configuration,
                    builder.Environment);

            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOpenApi();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            //rate limiting
            builder.Services.AddRateLimiting();

            //memory cache
            builder.Services.AddMemoryCache();

            var app = builder.Build();

            // ---------------------------- Database migration ----------------------------
            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "An error occurred while migrating or initializing the database");
                    throw;
                }
            }


            // ---------------------------- Middleware ----------------------------

            try
            {
                app.UseSerilogRequestLogging();
                app.UseMiddleware<ExceptionMiddleware>();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                    app.MapScalarApiReference(options =>
                    {
                        options
                            .WithTitle("Simple Service API")
                            .WithTheme(ScalarTheme.Default)
                            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
                    });
                }

                app.UseHttpsRedirection();
                app.UseCors("AllowAll");
                app.UseRateLimiter();
                //rate limiting
                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllers()
                    .RequireRateLimiting("IpPolicy"); 

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}