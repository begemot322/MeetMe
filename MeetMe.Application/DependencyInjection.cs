using MeetMe.Application.Services.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace MeetMe.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<EventService>();

        return services;
    }
}