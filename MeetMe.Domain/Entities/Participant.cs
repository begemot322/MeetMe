using System.ComponentModel.DataAnnotations;

namespace MeetMe.Domain.Entities;

public class Participant
{
    public int Id { get; set; }
    [Required]
    public string Nickname { get; set; }
    public bool IsCreator { get; set; }
    public bool? IsAgreedWithFixedDate { get; set; }

    public int EventId { get; set; }
    public Event Event { get; set; }
    
    public ICollection<DateRange> DateRanges { get; set; } = new List<DateRange>();
}