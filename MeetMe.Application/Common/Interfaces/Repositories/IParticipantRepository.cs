using MeetMe.Domain.Entities;

namespace MeetMe.Application.Common.Interfaces.Repositories;

public interface IParticipantRepository
{
    Task AddAsync(Participant participant);
    Task<Participant?> GetByEventIdAndNicknameAsync(int eventId, string nickname);
    
    Task UpdateAsync(Participant participant);


}