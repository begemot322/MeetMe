using MeetMe.Application.Dtos;
using MeetMe.Application.Services.Models;
using MeetMe.Domain.Entities;

namespace MeetMe.Application.Services.Interfaces;

public interface IEventService
{
    Task<int> CreateEventAsync(CreateEventDto EventDto);
    Task<Event> GetEventByIdAsync(int id);
    Task<Event> GetEventByCodeAsync(Guid code);
    Task<EventTimeSuggestion> CalculateBestTime(int eventId);
    Task<List<Event>> GetEventsCreatedByUserAsync();
}