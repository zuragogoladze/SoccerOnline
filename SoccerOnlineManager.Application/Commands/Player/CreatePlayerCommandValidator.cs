using FluentValidation;
using SoccerOnlineManager.Application.Exceptions;

namespace SoccerOnlineManager.Application.Commands.Player
{
    public class CreatePlayerCommandValidator : AbstractValidator<CreatePlayerCommand>
    {
        public CreatePlayerCommandValidator()
        {
            RuleFor(p => p.MarketValue).GreaterThanOrEqualTo(0).WithErrorCode(FieldExceptionCodes.NegativeValue);
            RuleFor(p => p.Age).GreaterThan(0).WithErrorCode(FieldExceptionCodes.NotPositiveValue);
            RuleFor(p => p.Position).NotNull().WithErrorCode(FieldExceptionCodes.Empty);
        }
    }
}
