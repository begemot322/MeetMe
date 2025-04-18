using MeetMe.Domain.Entities;

namespace MeetMe.Application.Common.Interfaces.Repositories;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(int id);
    Task<Event?> GetByCodeAsync(Guid code);
    Task AddAsync(Event ev);
}