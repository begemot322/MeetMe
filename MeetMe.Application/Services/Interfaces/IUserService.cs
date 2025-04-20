using MeetMe.Application.Dtos.Authentication;

namespace MeetMe.Application.Services.Interfaces;

public interface IUserService
{
    Task RegisterAsync(RegisterUserRequest registerUserRequest);
    Task<string> LoginAsync(LoginUserRequest loginUserRequest);
}