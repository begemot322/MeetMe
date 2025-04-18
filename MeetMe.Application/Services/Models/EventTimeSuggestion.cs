namespace MeetMe.Application.Services.Models;

public class EventTimeSuggestion
{
    public DateTime SuggestedStart { get; set; }
    public DateTime SuggestedEnd { get; set; }
    public int EventId { get; set; }
}