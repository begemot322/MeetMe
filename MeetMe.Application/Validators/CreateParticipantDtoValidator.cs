using FluentValidation;
using MeetMe.Application.Dtos;

namespace MeetMe.Application.Validators;

public class CreateParticipantDtoValidator : AbstractValidator<CreateParticipantDto>
{
    public CreateParticipantDtoValidator()
    {
        
        RuleFor(x => x).Custom((model, context) =>
        {
            if (model.IsAgreedToFixedDate && model.DateRanges.Any())
            {
                context.AddFailure("Нельзя указывать даты, если вы согласны на фиксированную дату");
            }

            if (!model.IsAgreedToFixedDate && (model.DateRanges == null || !model.DateRanges.Any()))
            {
                context.AddFailure("Если вы не согласны на фиксированную дату, необходимо указать хотя бы один диапазон дат");
            }
        });

        When(x => !x.IsAgreedToFixedDate && x.DateRanges != null, () =>
        {
            RuleForEach(x => x.DateRanges).ChildRules(range =>
            {
                range.RuleFor(r => r.StartDate)
                    .LessThan(r => r.EndDate)
                    .WithMessage("Дата начала диапазона должна быть раньше даты конца");

                range.RuleFor(r => r.StartDate)
                    .GreaterThan(DateTime.UtcNow)
                    .WithMessage("Дата начала диапазона должна быть в будущем");
            });
        });
    }
}