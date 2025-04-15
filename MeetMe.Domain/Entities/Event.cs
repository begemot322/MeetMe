using System.ComponentModel.DataAnnotations;

namespace MeetMe.Domain.Entities;

public class Event
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string CreatorNickname { get; set; } 
    public Guid Code { get; set; } = Guid.NewGuid();
    public DateTime? FixedDate { get; set; } 
    public bool IsDateRange { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<Participant> Participants { get; set; }
}