using FluentValidation;
using SoccerOnlineManager.Application.Exceptions;

namespace SoccerOnlineManager.Application.Commands.Player
{
    public class CreatePlayersCommandValidator : AbstractValidator<CreatePlayersCommand>
    {
        public CreatePlayersCommandValidator()
        {
            RuleFor(t => t.TeamId).NotEmpty().WithErrorCode(FieldExceptionCodes.Empty);
        }
    }
}
