using MediatR;
using Microsoft.Extensions.Options;
using SoccerOnlineManager.Application.Settings;
using SoccerOnlineManager.Infrastructure.Contexts;
using SoccerOnlineManager.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Application.Commands.Player
{
    public class CreatePlayersCommand : IRequest
    {
        public Guid TeamId { get; set; }
    }

    public class CreatePlayersCommandHandler : IRequestHandler<CreatePlayersCommand>
    {
        private readonly DatabaseContext _context;
        private readonly GameSettings _gameSettings;

        public CreatePlayersCommandHandler(DatabaseContext context, IOptions<GameSettings> gameOptions)
        {
            _context = context;
            _gameSettings = gameOptions.Value;
        }

        public async Task<Unit> Handle(CreatePlayersCommand command, CancellationToken cancellationToken)
        {
            var players = new List<Infrastructure.Entities.Player>();

            for (int i = 0; i < _gameSettings.TeamGoalkeepersCount; i++)
            {
                players.Add(CreatePlayer(command.TeamId, Position.Goalkeeper));
            }
            for (int i = 0; i < _gameSettings.TeamDefendersCount; i++)
            {
                players.Add(CreatePlayer(command.TeamId, Position.Defender));
            }
            for (int i = 0; i < _gameSettings.TeamMidfieldersCount; i++)
            {
                players.Add(CreatePlayer(command.TeamId, Position.Midfielder));
            }
            for (int i = 0; i < _gameSettings.TeamAttackersCount; i++)
            {
                players.Add(CreatePlayer(command.TeamId, Position.Attacker));
            }

            await _context.Players.AddRangeAsync(players, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private Infrastructure.Entities.Player CreatePlayer(Guid teamId, Position position)
        {
            var r = new Random();
            return new Infrastructure.Entities.Player
            {
                Age = r.Next(_gameSettings.PlayerMinAge, _gameSettings.PlayerMaxAge),
                MarketValue = _gameSettings.PlayerInitialValue,
                Position = position,
                TeamId = teamId
            };
        }
    }
}
