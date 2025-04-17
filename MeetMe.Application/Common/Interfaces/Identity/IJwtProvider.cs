using MeetMe.Domain.Entities;

namespace MeetMe.Application.Common.Interfaces.Identity;

public interface IJwtProvider
{
    string GenerateToken(User user);
}