using FluentValidation;
using SoccerOnlineManager.Application.Exceptions;

namespace SoccerOnlineManager.Application.Commands.Player
{
    public class UpdatePlayerCommandValidator : AbstractValidator<UpdatePlayerCommand>
    {
        public UpdatePlayerCommandValidator()
        {
            RuleFor(p => p.Id).NotEmpty().WithErrorCode(FieldExceptionCodes.Empty);
        }
    }
}
