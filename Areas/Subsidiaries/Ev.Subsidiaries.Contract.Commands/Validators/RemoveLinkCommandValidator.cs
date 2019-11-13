using FluentValidation;

namespace Ev.Subsidiaries.Contract.Commands.Validators
{
    public class RemoveLinkCommandValidator : AbstractValidator<RemoveLinkCommand>
    {
        public RemoveLinkCommandValidator()
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