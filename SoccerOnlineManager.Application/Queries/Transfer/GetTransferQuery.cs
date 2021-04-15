using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoccerOnlineManager.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Application.Queries.Transfer
{
    public class GetTransferQuery : IRequest<TransferDTO>
    {
        [FromRoute(Name = "id")]
        public Guid Id { get; set; }
    }

    public class GetTransferQueryHandler : IRequestHandler<GetTransferQuery, TransferDTO>
    {
        private readonly DatabaseContext _context;

        public GetTransferQueryHandler(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<TransferDTO> Handle(GetTransferQuery query, CancellationToken cancellationToken)
        {
            var transfer = await _context.Transfers
                .Include(t => t.Player)
                    .ThenInclude(p => p.Team)
                .FirstOrDefaultAsync(t => t.Id == query.Id);

            if (transfer == null)
                throw new KeyNotFoundException();

            var player = transfer.Player;

            return new TransferDTO(transfer.Id, player.FirstName, player.LastName, player.Country, transfer.Price, player.Team.Name);
        }
    }
}
