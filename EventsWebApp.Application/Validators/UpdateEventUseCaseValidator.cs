using EventsWebApp.Application.UseCases.Events.UpdateEvent;
using FluentValidation.Results;
using FluentValidation;

namespace EventsWebApp.Application.Validators;

public sealed class UpdateEventUseCaseValidator :
	AbstractValidator<UpdateEventUseCase>
{
	public UpdateEventUseCaseValidator()
	{
		RuleFor(c => c.Event.Name)
			.NotEmpty().WithMessage("Name is required.")
			.Length(1, 100).WithMessage("Name must be between 1 and 100 characters.");

		RuleFor(c => c.Event.MaxCountParticipants)
			.NotEmpty().WithMessage("MaxCountParticipants is required.")
			.GreaterThan(0).WithMessage("MaxCountParticipants must be greater than 0.");

		RuleFor(c => c.Event.DateTime)
			.NotEmpty().WithMessage("DateTime is required.");

		RuleFor(c => c.Event.Category)
			.NotEmpty().WithMessage("Category is required.")
			.Length(1, 250).WithMessage("Category must be between 1 and 250 characters.");

		RuleFor(c => c.Event.Location)
			.NotEmpty().WithMessage("Location is required.")
			.Length(1, 250).WithMessage("Location must be between 1 and 250 characters.");

		RuleFor(c => c.Event.Description)
			.Length(1, 500).WithMessage("Description must be between 1 and 500 characters.");
	}

	public override ValidationResult Validate(ValidationContext<UpdateEventUseCase> context)
	{
		return context.InstanceToValidate.Event is null
			? new ValidationResult([
				new ValidationFailure("EventForUpdateDto", "EventForUpdateDto object is null")
			]) : base.Validate(context);
	}

}

