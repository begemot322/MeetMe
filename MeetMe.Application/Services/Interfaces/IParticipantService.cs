using MeetMe.Application.Dtos;
using MeetMe.Domain.Entities;

namespace MeetMe.Application.Services.Interfaces;

public interface IParticipantService
{
    Task AddParticipantAsync(CreateParticipantDto dto);
    Task<Participant?> GetParticipantAsync(int eventId, string nickname);
}