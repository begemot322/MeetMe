using MeetMe.Application.Dtos.Authentication;
using MeetMe.Application.Services.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace MeetMe.Web.Controllers;

public class AuthController : Controller
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterUserRequest registerUserRequest)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _userService.RegisterAsync(registerUserRequest);
                TempData["success"] = "Вы успешно зарегистрировались";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }
        return View(registerUserRequest);
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        if (ModelState.IsValid)
        {
            try
            {
                string token = await _userService.LoginAsync(request.Email, request.Password);
                Response.Cookies.Append("SecurityCookies", token);
                TempData["success"] = "Вы успешно вошли в аккаунт";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }
        return View(request);
    }
    
    [HttpPost]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("SecurityCookies");

        return RedirectToAction("Login", "Auth");
    }

}