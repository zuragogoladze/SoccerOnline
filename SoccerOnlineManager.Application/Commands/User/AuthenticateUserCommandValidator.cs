using FluentValidation;
using SoccerOnlineManager.Application.Exceptions;

namespace SoccerOnlineManager.Application.Commands.User
{
    public class AuthenticateUserCommandValidator : AbstractValidator<AuthenticateUserCommand>
    {
        public AuthenticateUserCommandValidator()
        {
            RuleFor(p => p.Email).NotEmpty().WithErrorCode(FieldExceptionCodes.Empty);
            RuleFor(p => p.Password).NotEmpty().WithErrorCode(FieldExceptionCodes.Empty);
        }
    }
}
