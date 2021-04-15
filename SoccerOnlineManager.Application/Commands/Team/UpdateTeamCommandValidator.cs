using FluentValidation;
using SoccerOnlineManager.Application.Exceptions;

namespace SoccerOnlineManager.Application.Commands.Team
{
    public class UpdateTeamCommandValidator : AbstractValidator<UpdateTeamCommand>
    {
        public UpdateTeamCommandValidator()
        {
            RuleFor(t => t.Name).NotEmpty().WithErrorCode(FieldExceptionCodes.Empty);
            RuleFor(t => t.Country).NotEmpty().WithErrorCode(FieldExceptionCodes.Empty);
        }
    }
}
