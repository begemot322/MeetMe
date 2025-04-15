using MeetMe.Application.Dtos;
using MeetMe.Application.Services.Implementation;
using MeetMe.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MeetMe.Web.Controllers;

public class EventController : Controller
{
    private readonly EventService _eventService;

    public EventController(EventService eventService)
    {
        _eventService = eventService;
    }
    
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEventDto model)
    {
        if (!ModelState.IsValid)
            return View(model);
    
        var eventCode = await _eventService.CreateEventAsync(model);
        
        return RedirectToAction("Success", new { code = eventCode });
    }
    
    public async Task<IActionResult> Success(Guid code, string title)
    {
        var ev = await _eventService.GetEventByCodeAsync(code);

        var vm = new SuccessViewModel
        {
            Code = ev.Code,
            EventTitle = ev.Title
        };

        return View(vm);
    }
    
    [HttpGet]
    public IActionResult Join()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Join(JoinEventViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var ev = await _eventService.GetEventByCodeAsync(model.EventCode);
        
        return RedirectToAction("AddDates", new { code = model.EventCode, nickname = model.Nickname });
    }


    
}