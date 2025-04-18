using MeetMe.Application.Common.Exceptions;
using MeetMe.Application.Common.Interfaces.Identity;
using MeetMe.Application.Common.Interfaces.Repositories;
using MeetMe.Application.Dtos;
using MeetMe.Application.Services.Interfaces;
using MeetMe.Domain.Entities;

namespace MeetMe.Application.Services.Implementation;

public class ParticipantService : IParticipantService
{
    private readonly IEventRepository _eventRepository;
    private readonly IParticipantRepository _participantRepository;
    private readonly IUserContext _userContext;

    public ParticipantService(
        IEventRepository eventRepository,
        IParticipantRepository participantRepository,
        IUserContext userContext)
    {
        _eventRepository = eventRepository;
        _participantRepository = participantRepository;
        _userContext = userContext;
    }
    
    public async Task AddParticipantAsync(CreateParticipantDto dto)
    {
        var ev = await _eventRepository.GetByIdAsync(dto.EventId);

        if (ev == null)
            throw new NotFoundException($"Event with id {dto.EventId} not found.");

        var existingParticipant = await _participantRepository
            .GetByEventIdAndNicknameAsync(ev.Id, dto.Nickname);

        if (existingParticipant != null)
        {
            await UpdateExistingParticipant(existingParticipant, dto);
        }
        else
        {
            await CreateNewParticipant(ev.Id, dto);
        }
        
    }

    public async Task<Participant?> GetParticipantAsync(int eventId, string nickname)
    {
        return await _participantRepository.GetByEventIdAndNicknameAsync(eventId, nickname);
    }
    

    private async Task UpdateExistingParticipant(Participant participant, CreateParticipantDto dto)
    {
        participant.IsAgreedWithFixedDate = dto.IsAgreedToFixedDate;
        participant.DateRanges.Clear();

        foreach (var range in dto.DateRanges)
        {
            participant.DateRanges.Add(new DateRange
            {
                StartDate = range.StartDate,
                EndDate = range.EndDate
            });
        }

        await _participantRepository.UpdateAsync(participant);
    }

    private async Task CreateNewParticipant(int eventId, CreateParticipantDto dto)
    {
        var currentUserId = _userContext.GetCurrentUserId();

        var participant = new Participant
        {
            Nickname = dto.Nickname,
            IsCreator = false,
            EventId = eventId,
            IsAgreedWithFixedDate = dto.IsAgreedToFixedDate,
            UserId = currentUserId
        };

        foreach (var range in dto.DateRanges)
        {
            participant.DateRanges.Add(new DateRange
            {
                StartDate = range.StartDate,
                EndDate = range.EndDate
            });
        }

        await _participantRepository.AddAsync(participant);
    }
}
