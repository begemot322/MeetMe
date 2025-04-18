using MeetMe.Application.Common.Exceptions;
using MeetMe.Application.Common.Interfaces.Identity;
using MeetMe.Application.Common.Interfaces.Repositories;
using MeetMe.Application.Dtos;
using MeetMe.Application.Services.Interfaces;
using MeetMe.Application.Services.Models;
using MeetMe.Domain.Entities;

namespace MeetMe.Application.Services.Implementation;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IParticipantRepository _participantRepository;
    private readonly IUserContext _userContext;
    private readonly IEventTimeCalculationService _timeCalculationService;


    public EventService(
        IEventRepository eventRepository,
        IParticipantRepository participantRepository,
        IUserContext userContext,
        IEventTimeCalculationService timeCalculationService)
    {
        _eventRepository = eventRepository;
        _participantRepository = participantRepository;
        _userContext = userContext;
        _timeCalculationService = timeCalculationService;
    }

    public async Task<int> CreateEventAsync(CreateEventDto EventDto)
    {
        var currentUserId = _userContext.GetCurrentUserId();

        if (currentUserId == null)
            throw new UnauthorizedAccessException("User must be authenticated to create an event.");

        var newEvent = new Event
        {
            Title = EventDto.Title,
            CreatorNickname = EventDto.CreatorNickname,
            FixedDate = EventDto.FixedDate,
            IsDateRange = EventDto.IsDateRange
        };

        await _eventRepository.AddAsync(newEvent);

        var creator = new Participant
        {
            Nickname = EventDto.CreatorNickname,
            IsCreator = true,
            Event = newEvent,
            IsAgreedWithFixedDate = true,
            UserId = currentUserId.Value
        };

        if (EventDto.IsDateRange)
        {
            foreach (var range in EventDto.DateRanges)
            {
                creator.DateRanges.Add(new DateRange
                {
                    StartDate = range.StartDate,
                    EndDate = range.EndDate
                });
            }
        }

        await _participantRepository.AddAsync(creator);

        return newEvent.Id;
    }

    public async Task<Event> GetEventByIdAsync(int id)
    {
        var ev = await _eventRepository.GetByIdAsync(id);

        if (ev == null)
            throw new NotFoundException($"Event with id {id} not found.");

        return ev;
    }
    public async Task<Event> GetEventByCodeAsync(Guid code)
    {
        var ev = await _eventRepository.GetByCodeAsync(code);

        if (ev == null)
            throw new NotFoundException($"Event with code {code} not found.");

        return ev;
    }
    

    public async Task<EventTimeSuggestion> CalculateBestTime(int eventId)
    {
        return await _timeCalculationService.CalculateBestTime(eventId);
    }
    
    public async Task<List<Event>> GetEventsCreatedByUserAsync()
    {
        var currentUserId = _userContext.GetCurrentUserId();
        
        return await _participantRepository.GetCreatedEventsByUserIdAsync(currentUserId);
    }
}