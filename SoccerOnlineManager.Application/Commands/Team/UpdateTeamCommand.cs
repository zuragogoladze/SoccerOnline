using MediatR;
using SoccerOnlineManager.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Application.Commands.Team
{
    public class UpdateTeamCommand : IRequest
    {
        public string Name { get; set; }

        public string Country { get; set; }

        public decimal? TransferBudget { get; set; }

        public Guid TeamId { get; private set; }

        public UpdateTeamCommand(string name, string country, decimal? transferBudget, Guid teamId)
        {
            Name = name;
            Country = country;
            TransferBudget = transferBudget;
            TeamId = teamId;
        }
    }

    public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand>
    {
        private readonly DatabaseContext _context;

        public UpdateTeamCommandHandler(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateTeamCommand command, CancellationToken cancellationToken)
        {
            var team = _context.Teams.FirstOrDefault(t => t.UserId == command.TeamId);
            if (team == null)
                throw new KeyNotFoundException();

            team.Name = command.Name;
            team.Country = command.Country;

            if (command.TransferBudget.HasValue)
                team.TransferBudget = command.TransferBudget.Value;

            _context.Update(team);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
