using FluentValidation;
using SoccerOnlineManager.Application.Exceptions;

namespace SoccerOnlineManager.Application.Queries.Team
{
    public class GetTeamWithPlayersQueryValidator : AbstractValidator<GetTeamWithPlayersQuery>
    {
        public GetTeamWithPlayersQueryValidator()
        {
            RuleFor(p => p.TeamId).NotEmpty().WithErrorCode(FieldExceptionCodes.Empty);
        }
    }
}
