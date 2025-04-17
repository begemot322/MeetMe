using MeetMe.Application.Common.Exceptions;
using MeetMe.Application.Common.Interfaces.Identity;
using MeetMe.Application.Common.Interfaces.Repositories;
using MeetMe.Application.Dtos.Authentication;
using MeetMe.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace MeetMe.Application.Services.Implementation;

public class UserService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;

    public UserService(IPasswordHasher passwordHasher, IUserRepository userRepository,
        IJwtProvider jwtProvider)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }
    
    public async Task RegisterAsync(RegisterUserRequest registerUserRequest)
    {
        bool emailExists = await _userRepository.ExistsAsync(u => u.Email == registerUserRequest.Email);
        if (emailExists)
        {
            throw new DuplicateException("Пользователь с таким email уже существует.");
        }
        
        var hashedPassword = _passwordHasher.Generate(registerUserRequest.Password);
        
        var user = new User
        {
            UserName = registerUserRequest.UserName,
            Email = registerUserRequest.Email,
            PasswordHash = hashedPassword,
        };

        await _userRepository.AddAsync(user);
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByExpressionAsync(u => u.Email == email);

        if (user == null)
        {
            throw new Exception("Неверный логин или пароль.");
        }

        var result = _passwordHasher.Verify(password, user.PasswordHash);


        if (result == false)
        {
            throw new Exception("Неверный логин или пароль");
        }

        var token = _jwtProvider.GenerateToken(user);

        return token;
    }
}