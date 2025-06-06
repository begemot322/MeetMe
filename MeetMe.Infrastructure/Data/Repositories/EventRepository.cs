﻿using MeetMe.Application.Common.Interfaces.Repositories;
using MeetMe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Infrastructure.Data.Repositories;

public class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext _context;

    public EventRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Event?> GetByIdAsync(int eventId)
    {
        return await _context.Events
            .Include(e => e.Participants)
            .ThenInclude(p => p.DateRanges)
            .FirstOrDefaultAsync(e => e.Id == eventId);    
    }

    public async Task<Event?> GetByCodeAsync(Guid code)
    {
        return await _context.Events
            .Include(e => e.Participants)
            .ThenInclude(p => p.DateRanges)
            .FirstOrDefaultAsync(e => e.Code == code);
    }

    public async Task AddAsync(Event ev)
    {
        await _context.Events.AddAsync(ev);
        await _context.SaveChangesAsync();
    }
}