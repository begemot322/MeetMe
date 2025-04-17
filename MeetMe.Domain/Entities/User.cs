namespace MeetMe.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    
    public ICollection<Participant> Participants { get; set; } = new List<Participant>();
}