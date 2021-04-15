using FluentValidation;
using SoccerOnlineManager.Application.Exceptions;
using SoccerOnlineManager.Application.Helpers;

namespace SoccerOnlineManager.Application.Commands.User
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty().WithErrorCode(FieldExceptionCodes.Empty)
                .Must(EmailHelper.IsValid).WithErrorCode(FieldExceptionCodes.InvalidEmail);
            RuleFor(p => p.Password).NotEmpty().WithErrorCode(FieldExceptionCodes.Empty);
        }
    }
}
