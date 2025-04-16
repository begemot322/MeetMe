namespace MeetMe.Application.Dtos;

public class CreateParticipantDto
{
    public Guid EventCode { get; set; }
    public string Nickname { get; set; }
    public bool IsAgreedToFixedDate { get; set; }
    public List<CreateDateRangeDto> DateRanges { get; set; } = new();
}