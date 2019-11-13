using FluentValidation;

namespace Ev.Subsidiaries.Contract.Commands.Validators
{
    public class CreatePendingLinkCommandValidator : AbstractValidator<CreatePendingLinkCommand>
    {
        public CreatePendingLinkCommandValidator()
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