using MeetMe.Domain.Entities;

namespace MeetMe.Application.Common.Interfaces.Repositories;

public interface IParticipantRepository
{
    Task AddAsync(Participant participant);

}