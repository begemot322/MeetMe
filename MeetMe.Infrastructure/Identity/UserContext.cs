using MeetMe.Application.Common.Interfaces.Identity;
using Microsoft.AspNetCore.Http;

namespace MeetMe.Infrastructure.Identity;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?
            .FindFirst("userId")?.Value;

        return int.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}