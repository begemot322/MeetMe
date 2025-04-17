using System.ComponentModel.DataAnnotations;

namespace MeetMe.Application.Dtos.Authentication;

public class RegisterUserRequest
{
    
    [Required]
    public string UserName { get; set; }
    [Required(ErrorMessage = "Email обязателен.")]
    [EmailAddress(ErrorMessage = "Некорректный формат Email.")]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}