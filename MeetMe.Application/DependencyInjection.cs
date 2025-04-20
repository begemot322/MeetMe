using FluentValidation;
using FluentValidation.AspNetCore;
using MeetMe.Application.Services.Implementation;
using MeetMe.Application.Services.Interfaces;
using MeetMe.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace MeetMe.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEventTimeCalculationService, EventTimeCalculationService>();
        services.AddScoped<IParticipantService, ParticipantService>();
        
        services.AddFluentValidationAutoValidation(); 
        services.AddValidatorsFromAssemblyContaining<CreateEventDtoValidator>();

        return services;
    }
}