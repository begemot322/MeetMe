using MeetMe.Application.Common.Interfaces.Repositories;
using MeetMe.Domain.Entities;

namespace MeetMe.Infrastructure.Data.Repositories;

public class ParticipantRepository : IParticipantRepository
{
    private readonly ApplicationDbContext _context;

    public ParticipantRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Participant participant)
    {
        await _context.Participants.AddAsync(participant);
        await _context.SaveChangesAsync();
    }
}