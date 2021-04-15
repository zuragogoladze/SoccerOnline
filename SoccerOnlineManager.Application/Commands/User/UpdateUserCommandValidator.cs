using FluentValidation;
using SoccerOnlineManager.Application.Exceptions;
using SoccerOnlineManager.Application.Helpers;

namespace SoccerOnlineManager.Application.Commands.User
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty().WithErrorCode(FieldExceptionCodes.Empty)
                .Must(EmailHelper.IsValid).WithErrorCode(FieldExceptionCodes.InvalidEmail);
        }
    }
}
