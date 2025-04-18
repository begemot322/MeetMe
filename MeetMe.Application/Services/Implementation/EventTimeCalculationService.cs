using MeetMe.Application.Common.Exceptions;
using MeetMe.Application.Common.Interfaces.Repositories;
using MeetMe.Application.Services.Interfaces;
using MeetMe.Application.Services.Models;
using MeetMe.Domain.Entities;

namespace MeetMe.Application.Services.Implementation;

public class EventTimeCalculationService : IEventTimeCalculationService
{
    private readonly IEventRepository _eventRepository;

    public EventTimeCalculationService(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }
    public async Task<EventTimeSuggestion> CalculateBestTime(int eventId)
    {
        var ev = await _eventRepository.GetByIdAsync(eventId);

        if (ev == null)
            throw new NotFoundException($"Event with Id {eventId} not found.");

        if (ev.FixedDate.HasValue && 
            ev.Participants.All(p => p.IsAgreedWithFixedDate == true))
        {
            return new EventTimeSuggestion
            {
                SuggestedStart = ev.FixedDate.Value,
                SuggestedEnd = ev.FixedDate.Value.AddHours(1),
                EventId = eventId
            };
        }

        var participants = ev.Participants
            .Where(p => p.DateRanges.Any())
            .ToList();

        if (participants.Count < 2)
            throw new InvalidOperationException("Нужно минимум 2 участниками с диапазоном дат");

        var allPossibleSlots = FindCommonTimeSlots(participants);

        if (!allPossibleSlots.Any())
            throw new InvalidOperationException("Нет общих дат");

        var bestSlot = allPossibleSlots
            .OrderByDescending(s => s.End - s.Start)
            .ThenBy(s => s.Start)
            .First();

        return new EventTimeSuggestion()
        {
            SuggestedStart = bestSlot.Start,
            SuggestedEnd = bestSlot.End,
            EventId = eventId
        };
    }

    private List<TimeSlot> FindCommonTimeSlots(List<Participant> participants)
    {
        var result = new List<TimeSlot>();

        var allRanges = participants
            .SelectMany(p => p.DateRanges)
            .OrderBy(r => r.StartDate)
            .ToList();

        for (int i = 0; i < allRanges.Count; i++)
        {
            for (int j = i + 1; j < allRanges.Count; j++)
            {
                var range1 = allRanges[i];
                var range2 = allRanges[j];

                var maxStart = range1.StartDate > range2.StartDate ? range1.StartDate : range2.StartDate;
                var minEnd = range1.EndDate < range2.EndDate ? range1.EndDate : range2.EndDate;

                if (maxStart < minEnd)
                {
                    bool allParticipantsAvailable = participants.All(p =>
                        p.DateRanges.Any(r =>
                            r.StartDate <= maxStart && r.EndDate >= minEnd));

                    if (allParticipantsAvailable)
                    {
                        var existingSlot = result.FirstOrDefault(s =>
                            (s.Start <= maxStart && s.End >= maxStart) ||
                            (s.Start <= minEnd && s.End >= minEnd));

                        if (existingSlot == null)
                        {
                            result.Add(new TimeSlot { Start = maxStart, End = minEnd });
                        }
                        else
                        {
                            existingSlot.Start = existingSlot.Start < maxStart ? existingSlot.Start : maxStart;
                            existingSlot.End = existingSlot.End > minEnd ? existingSlot.End : minEnd;
                        }
                    }
                }
            }
        }

        return result;
    }
    
}
