using MeetMe.Application.Common.Interfaces.Repositories;
using MeetMe.Infrastructure.Data;
using MeetMe.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeetMe.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);

        return services;
    }
    
    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connString));

        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IParticipantRepository, ParticipantRepository>();
        
        return services;
    }
    
}