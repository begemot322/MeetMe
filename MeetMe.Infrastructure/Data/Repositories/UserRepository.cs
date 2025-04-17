using System.Linq.Expressions;
using MeetMe.Application.Common.Interfaces.Repositories;
using MeetMe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<User?> GetByExpressionAsync(Expression<Func<User, bool>> predicate)
    {
        return await _db.Users
            .Where(predicate)
            .AsNoTracking()
            .FirstOrDefaultAsync();    }

    public async Task AddAsync(User user)
    {
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
    }

    public async Task Update(User user)
    { 
        _db.Users.Update(user);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate)
    {
        return await _db.Users.AnyAsync(predicate);  
    }
}