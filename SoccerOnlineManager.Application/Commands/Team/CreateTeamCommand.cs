using MediatR;
using Microsoft.Extensions.Options;
using SoccerOnlineManager.Application.Commands.Player;
using SoccerOnlineManager.Application.Exceptions;
using SoccerOnlineManager.Application.Settings;
using SoccerOnlineManager.Infrastructure.Contexts;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Application.Commands.Team
{
    public class CreateTeamCommand : IRequest
    {
        public Guid UserId { get; set; }
    }

    public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand>
    {
        private readonly DatabaseContext _context;
        private readonly IMediator _mediator;
        private readonly GameSettings _gameSettings;

        public CreateTeamCommandHandler(DatabaseContext context, IOptions<GameSettings> gameOptions, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
            _gameSettings = gameOptions.Value;
        }

        public async Task<Unit> Handle(CreateTeamCommand command, CancellationToken cancellationToken)
        {
            if (_context.Teams.Any(x => x.UserId == command.UserId))
                throw new ApiException(ExceptionCodes.UserHasTeam);

            var team = new Infrastructure.Entities.Team
            {
                UserId = command.UserId,
                TransferBudget = _gameSettings.TeamInitialTransferBudget
            };

            await _context.Teams.AddAsync(team, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Send(new CreatePlayersCommand { TeamId = team.UserId });

            return Unit.Value;
        }
    }
}
