using FluentValidation;

namespace Ev.Subsidiaries.Contract.Commands.Validators
{
    public class CreateAcceptedLinkCommandValidator : AbstractValidator<CreateAcceptedLinkCommand>
    {
        public CreateAcceptedLinkCommandValidator()
        {
            RuleFor(x => x.ParentId)
                .NotNull();
            RuleFor(x => x.SubsidiaryIds)
                .NotNull().NotEmpty();
            RuleForEach(x => x.SubsidiaryIds)
                .NotNull();
        }
    }
}