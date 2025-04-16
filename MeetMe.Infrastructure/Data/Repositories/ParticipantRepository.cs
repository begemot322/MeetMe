using MeetMe.Application.Common.Interfaces.Repositories;
using MeetMe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Participant?> GetByEventIdAndNicknameAsync(int eventId, string nickname)
    {
        return await _context.Participants
            .Include(p => p.DateRanges)
            .FirstOrDefaultAsync(p => p.EventId == eventId && p.Nickname == nickname);
    }
    
    public async Task UpdateAsync(Participant participant)
    {
        _context.Participants.Update(participant);
        await _context.SaveChangesAsync();
    }
}