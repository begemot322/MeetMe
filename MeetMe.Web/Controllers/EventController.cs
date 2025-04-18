using MeetMe.Application.Common.Exceptions;
using MeetMe.Application.Dtos;
using MeetMe.Application.Services.Implementation;
using MeetMe.Application.Services.Interfaces;
using MeetMe.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MeetMe.Web.Controllers;

public class EventController : Controller
{
    private readonly IEventService _eventService;
    private readonly IParticipantService _participantService;
    
    public EventController(
        IEventService eventService,
        IParticipantService participantService)
    {
        _eventService = eventService;
        _participantService = participantService;
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
    
        var eventId = await _eventService.CreateEventAsync(model);
        TempData["success"] = "Вы успешно добавили событие";
        
        return RedirectToAction("Success", new { eventId = eventId });
    }
    
    public async Task<IActionResult> Success(int eventId)
    {
        var ev = await _eventService.GetEventByIdAsync(eventId);

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
        
        return RedirectToAction("AddDates", new { eventId = ev.Id, nickname = model.Nickname });
    }
    
    [HttpGet]
    public async Task<IActionResult> AddDates(int eventId, string nickname)
    {
        var ev = await _eventService.GetEventByIdAsync(eventId);

        ViewBag.Title = ev.Title;
        ViewBag.FixedDate = ev.FixedDate;
        
        var participantDto = new CreateParticipantDto
        {
            EventId = eventId,
            Nickname = nickname,
            DateRanges = new List<CreateDateRangeDto>()
        };
            
        return View(participantDto);
    }

    [HttpPost]
    public async Task<IActionResult> AddDates(CreateParticipantDto model)
    {
        if (!ModelState.IsValid)
        {
            var ev = await _eventService.GetEventByIdAsync(model.EventId);
            ViewBag.Title = ev.Title;
            ViewBag.FixedDate = ev.FixedDate;
            return View(model);
        }
        
        await _participantService.AddParticipantAsync(model);
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
    
    [HttpGet]
    public async Task<IActionResult> CalculateBestTime(int eventId)
    {
        try
        {
            var result = await _eventService.CalculateBestTime(eventId);
            var ev = await _eventService.GetEventByIdAsync(eventId);
        
            var vm = new EventTimeSuggestionViewModel
            {
                ev = ev,
                SuggestionDto = result
            };
            return View("TimeSuggestion", vm);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }
        var eventDetails = await _eventService.GetEventByIdAsync(eventId);
        return View("Details", eventDetails);
    }
    
    [HttpGet]
    public async Task<IActionResult> MyEvents()
    {
        var events = await _eventService.GetEventsCreatedByUserAsync();
        return View(events);
    }
    
}