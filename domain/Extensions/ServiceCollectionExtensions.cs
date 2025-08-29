using Domain.Data;
using Domain.Interfaces;
using Domain.Repositories;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure Entity Framework
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Register repositories
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITackleRepository, TackleRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IFishSpeciesRepository, FishSpeciesRepository>();
        services.AddScoped<IFishingReportRepository, FishingReportRepository>();
        
        // Register services
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITackleService, TackleService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<IFishSpeciesService, FishSpeciesService>();
        services.AddScoped<IFishingReportService, FishingReportService>();
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}