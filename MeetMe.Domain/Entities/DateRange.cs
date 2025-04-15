using System.ComponentModel.DataAnnotations;

namespace MeetMe.Domain.Entities;

public class DateRange
{
    public int Id { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }
    
    public int ParticipantId { get; set; }
    public Participant Participant { get; set; }
}