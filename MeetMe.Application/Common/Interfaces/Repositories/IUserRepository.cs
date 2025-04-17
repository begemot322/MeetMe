using System.Linq.Expressions;
using MeetMe.Domain.Entities;

namespace MeetMe.Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByExpressionAsync(Expression<Func<User, bool>> predicate);

    Task AddAsync(User user);

    Task Update(User user);
    Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate);
}