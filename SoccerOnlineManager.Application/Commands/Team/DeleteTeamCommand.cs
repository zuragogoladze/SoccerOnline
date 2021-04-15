using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoccerOnlineManager.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Application.Commands.Team
{
    public class DeleteTeamCommand : IRequest
    {
        [FromRoute(Name = "id")]
        public Guid Id { get; set; }
    }

    public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand>
    {
        private readonly DatabaseContext _context;

        public DeleteTeamCommandHandler(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteTeamCommand command, CancellationToken cancellationToken)
        {
            var team = _context.Teams.Find(command.Id);

            if (team == null)
                throw new KeyNotFoundException();

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
