using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoccerOnlineManager.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Application.Queries.Player
{
    public class GetPlayerQuery : IRequest<PlayerDTO>
    {
        [FromRoute(Name = "id")]
        public Guid Id { get; set; }
    }

    public class GetPlayerQueryHandler : IRequestHandler<GetPlayerQuery, PlayerDTO>
    {
        private readonly DatabaseContext _context;

        public GetPlayerQueryHandler(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<PlayerDTO> Handle(GetPlayerQuery query, CancellationToken cancellationToken)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == query.Id);

            if (player == null)
                throw new KeyNotFoundException();

            return new PlayerDTO(player.Id, player.Position, player.FirstName, player.LastName, player.Country, player.Age, player.MarketValue);
        }
    }
}
