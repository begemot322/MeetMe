using System.ComponentModel.DataAnnotations;

namespace MeetMe.Application.Dtos;

public class CreateEventDto
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string CreatorNickname { get; set; }

    public DateTime? FixedDate { get; set; }

    public bool IsDateRange { get; set; }

    public List<CreateDateRangeDto> DateRanges { get; set; } = new List<CreateDateRangeDto>();
}


