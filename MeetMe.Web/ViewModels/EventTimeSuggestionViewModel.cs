using MeetMe.Application.Services.Models;
using MeetMe.Domain.Entities;

namespace MeetMe.Web.ViewModels;

public class EventTimeSuggestionViewModel
{
    public EventTimeSuggestion SuggestionDto { get; set; }
    public Event ev { get; set; }
}