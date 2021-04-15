using MediatR;
using SoccerOnlineManager.Application.Exceptions;
using SoccerOnlineManager.Infrastructure.Contexts;
using SoccerOnlineManager.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Application.Commands.Transfer
{
    public class CreateTransferCommand : IRequest
    {
        public Guid PlayerId { get; set; }

        public decimal Price { get; set; }

        public bool IsAdmin { get; private set; }

        public Guid UserId { get; private set; }

        public CreateTransferCommand(Guid playerId, decimal price, bool isAdmin, Guid userId)
        {
            PlayerId = playerId;
            Price = price;
            IsAdmin = isAdmin;
            UserId = userId;
        }
    }

    public class CreateTransferCommandHandler : IRequestHandler<CreateTransferCommand>
    {
        private readonly DatabaseContext _context;

        public CreateTransferCommandHandler(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateTransferCommand command, CancellationToken cancellationToken)
        {
            var player = _context.Players.FirstOrDefault(t => t.Id == command.PlayerId);
            if (player == null)
                throw new KeyNotFoundException();

            if (_context.Transfers.Any(t => t.PlayerId == player.Id))
                throw new ApiException(ExceptionCodes.AlreadyOnTransfer);

            if (command.UserId != player.TeamId && !command.IsAdmin)
                throw new ApiException(HttpStatusCode.Forbidden);

            var transfer = new Infrastructure.Entities.Transfer
            {
                PlayerId = command.PlayerId,
                FromTeam = player.TeamId,
                Price = command.Price,
                Status = TransferStatus.Active
            };

            await _context.Transfers.AddAsync(transfer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
