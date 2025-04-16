using MeetMe.Application.Common.Exceptions;
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
        TempData["success"] = "Вы успешно добавили событие";
        
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
    
    [HttpGet]
    public async Task<IActionResult> AddDates(Guid code, string nickname)
    {
        var ev = await _eventService.GetEventByCodeAsync(code);

        ViewBag.Title = ev.Title;
        ViewBag.FixedDate = ev.FixedDate;
        
        var participantDto = new CreateParticipantDto
        {
            EventCode = code,
            Nickname = nickname,
            DateRanges = new List<CreateDateRangeDto>()
        };
            
        return View(participantDto);
    }

    [HttpPost]
    public async Task<IActionResult> AddDates(CreateParticipantDto model)
    {
        if (!ModelState.IsValid)
            return View(model);
        
        await _eventService.AddParticipantAsync(model);
        TempData["success"] = "Вы успешно добавили удобное время";

        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    public async Task<IActionResult> Details(Guid code)
    {
        try
        {
            var eventDetails = await _eventService.GetEventByCodeAsync(code);
            return View(eventDetails);
        }
        catch (NotFoundException ex)
        {
            return View("EventNotFound", code);
        }
    }
    
}