using FluentValidation;
using SoccerOnlineManager.Application.Exceptions;

namespace SoccerOnlineManager.Application.Commands.Transfer
{
    public class CreateTransferCommandValidator : AbstractValidator<CreateTransferCommand>
    {
        public CreateTransferCommandValidator()
        {
            RuleFor(t => t.PlayerId).NotEmpty().WithErrorCode(FieldExceptionCodes.Empty);
            RuleFor(t => t.Price).NotEmpty().WithErrorCode(FieldExceptionCodes.Empty)
                                 .GreaterThan(0).WithErrorCode(FieldExceptionCodes.NegativeValue);
        }
    }
}
