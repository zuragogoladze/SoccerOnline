using MediatR;
using SoccerOnlineManager.Application.Exceptions;
using SoccerOnlineManager.Infrastructure.Contexts;
using SoccerOnlineManager.Infrastructure.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Application.Commands.Player
{
    public class CreatePlayerCommand : IRequest
    {
        public Guid TeamId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Country { get; set; }

        public decimal MarketValue { get; set; }

        public int Age { get; set; }

        public Position? Position { get; set; }
    }

    public class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand>
    {
        private readonly DatabaseContext _context;

        public CreatePlayerCommandHandler(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreatePlayerCommand command, CancellationToken cancellationToken)
        {
            if (!_context.Teams.Any(t => t.UserId == command.TeamId))
                throw new ApiException(ExceptionCodes.NoTeam);

            var player = new Infrastructure.Entities.Player
            {
                TeamId = command.TeamId,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Age = command.Age,
                Country = command.Country,
                MarketValue = command.MarketValue,
                Position = command.Position.Value
            };

            await _context.Players.AddAsync(player, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
