using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoccerOnlineManager.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Application.Commands.Player
{
    public class DeletePlayerCommand : IRequest
    {
        [FromRoute(Name = "id")]
        public Guid Id { get; set; }
    }

    public class DeletePlayerCommandHandler : IRequestHandler<DeletePlayerCommand>
    {
        private readonly DatabaseContext _context;

        public DeletePlayerCommandHandler(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeletePlayerCommand command, CancellationToken cancellationToken)
        {
            var player = _context.Players.Find(command.Id);

            if (player == null)
                throw new KeyNotFoundException();

            _context.Players.Remove(player);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
