namespace MeetMe.Application.Common.Interfaces.Identity;

public interface IUserContext
{
    int? GetCurrentUserId();
}