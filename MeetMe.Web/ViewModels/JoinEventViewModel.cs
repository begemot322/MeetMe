using System.ComponentModel.DataAnnotations;

namespace MeetMe.Web.ViewModels;

public class JoinEventViewModel
{
    [Required(ErrorMessage = "Код события обязателен")]
    public Guid EventCode { get; set; }

    [Required(ErrorMessage = "Никнейм обязателен")]
    public string Nickname { get; set; }
}
