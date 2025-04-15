using FluentValidation;
using MeetMe.Application.Dtos;

namespace MeetMe.Application.Validators;

public class CreateEventDtoValidator: AbstractValidator<CreateEventDto>
{
    public CreateEventDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        
        RuleFor(x => x).Custom((model, context) =>
        {
            if (model.FixedDate.HasValue && model.IsDateRange && model.DateRanges?.Any() == true)
            {
                context.AddFailure("Нельзя указывать одновременно фиксированную дату и диапазон дат");
            }

            if (!model.FixedDate.HasValue && (!model.IsDateRange || model.DateRanges == null || !model.DateRanges.Any()))
            {
                context.AddFailure("Необходимо указать либо фиксированную дату, либо диапазон дат");
            }
        });

        When(x => x.FixedDate.HasValue, () =>
        {
            RuleFor(x => x.FixedDate).Must(date => date > DateTime.UtcNow)
                .WithMessage("Дата должна быть в будущем");
        });

        When(x => x.IsDateRange && x.DateRanges != null, () =>
        {
            RuleForEach(x => x.DateRanges).ChildRules(range =>
            {
                range.RuleFor(r => r.StartDate).LessThan(r => r.EndDate)
                    .WithMessage("Дата начала диапазона должна быть раньше даты конца");
                
                range.RuleFor(r => r.StartDate).GreaterThan(DateTime.UtcNow)
                    .WithMessage("Даты диапазона должны быть в будущем");
            });
        });
    }
}