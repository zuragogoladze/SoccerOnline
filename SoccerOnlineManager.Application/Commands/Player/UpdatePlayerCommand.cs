using MediatR;
using SoccerOnlineManager.Application.Exceptions;
using SoccerOnlineManager.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Application.Commands.Player
{
    public class UpdatePlayerCommand : IRequest
    {
        public Guid Id { get; private set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Country { get; set; }

        public decimal? MarketValue { get; set; }

        public Guid UserId { get; private set; }

        public bool IsAdmin { get; private set; }

        public UpdatePlayerCommand(Guid id, string firstName, string lastName, string country, decimal? marketValue, Guid userId, bool isAdmin)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Country = country;
            MarketValue = marketValue;
            UserId = userId;
            IsAdmin = isAdmin;
        }
    }

    public class UpdatePlayerCommandHandler : IRequestHandler<UpdatePlayerCommand>
    {
        private readonly DatabaseContext _context;

        public UpdatePlayerCommandHandler(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdatePlayerCommand command, CancellationToken cancellationToken)
        {
            var player = _context.Players.FirstOrDefault(t => t.Id == command.Id);
            if (player == null)
                throw new KeyNotFoundException();

            if (command.UserId != player.TeamId && !command.IsAdmin)
                throw new ApiException(HttpStatusCode.Forbidden);

            player.FirstName = command.FirstName;
            player.LastName = command.LastName;
            player.Country = command.Country;

            if (command.MarketValue.HasValue)
                player.MarketValue = command.MarketValue.Value;

            _context.Update(player);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
