using MeetMe.Application.Common.Interfaces.Repositories;
using MeetMe.Application.Dtos;
using MeetMe.Domain.Entities;

namespace MeetMe.Application.Services.Implementation;

public class EventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IParticipantRepository _participantRepository;
    
    public EventService(IEventRepository eventRepository, 
        IParticipantRepository participantRepository)
    {
        _eventRepository = eventRepository;
        _participantRepository = participantRepository;
    }

    public async Task<Guid> CreateEventAsync(CreateEventDto EventDto)
    {
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
            Event = newEvent
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

        return newEvent.Code;
    }

    public async Task<Event> GetEventByCodeAsync(Guid code)
    {
        var ev = await _eventRepository.GetByCodeAsync(code);

        if (ev == null)
            throw new InvalidOperationException("Amenity with code {code} not found.");

        return ev;
    }
}