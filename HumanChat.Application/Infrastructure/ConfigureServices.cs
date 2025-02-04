using System.Configuration;
using System.Reflection;
using HumanChat.Application.Common.Interfaces;
using HumanChat.Application.Common.Providers;
using HumanChat.Application.Infrastructure.Persistence;
using HumanChat.Application.Infrastructure.Persistence.Interceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HumanChat.Application.Infrastructure;

/// <summary>
///     Infrastructure configurations
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    ///     Adds and configure all dependency services
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection" /> where should services be added</param>
    /// <param name="configuration"><see cref="IConfiguration" /> as a source for application configuration</param>
    /// <param name="assembly">assembly <see cref="Assembly" /> to get services from</param>
    /// <param name="isDevelopment"><see cref="bool" /> indicating if application is running in development environment</param>
    /// <returns><see cref="IServiceCollection" /> with all dependency services added</returns>
    internal static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration,
                                                         Assembly? assembly = null, bool isDevelopment = false)
    {
        var connectionString = configuration.GetConnectionString("Database");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ConfigurationErrorsException(
                "DB Connection string as configured property named 'ConnectionStrings:Database' is missing");

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        services.AddDatabase(connectionString, isDevelopment);

        return services;
    }

    /// <summary>
    ///     Adds default development CORS policy to allow DEV Frontend app origin and any localhost origins
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection" /> where CORS policy should be added</param>
    /// <returns><see cref="IServiceCollection" /> with added default CORS policy</returns>
    internal static IServiceCollection AddDevelopmentCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.SetIsOriginAllowed(origin =>
                {
                    var uri = new Uri(origin);
                    var isLocalhost = uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase) ||
                                      uri.Host.Equals("127.0.0.1", StringComparison.OrdinalIgnoreCase);

                    return isLocalhost;
                }).AllowAnyHeader().AllowAnyMethod();
            });
        });

        return services;
    }

    /// <summary>
    ///     Adds <see cref="HumanChatDbContext" /> as scoped to services
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection" /> where should be database added</param>
    /// <param name="connectionString"><see cref="string" /> database connection string</param>
    /// <param name="isDevelopment"><see cref="bool" /> indicating if application is running in development environment</param>
    /// <returns><see cref="IServiceCollection" /> with database added</returns>
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString,
                                                 bool isDevelopment = false)
    {
        services.AddScoped<TrackingInterceptor>();
        services.AddDbContext<HumanChatDbContext>(options =>
        {
            if (!isDevelopment)
                return;

            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });

        return services;
    }
}