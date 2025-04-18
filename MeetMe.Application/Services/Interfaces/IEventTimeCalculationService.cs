using MeetMe.Application.Services.Models;

namespace MeetMe.Application.Services.Interfaces;

public interface IEventTimeCalculationService
{
    Task<EventTimeSuggestion> CalculateBestTime(int eventId);
}