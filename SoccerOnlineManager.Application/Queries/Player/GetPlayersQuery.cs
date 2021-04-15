using MediatR;
using SoccerOnlineManager.Infrastructure.Contexts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Application.Queries.Player
{
    public class GetPlayersQuery : IRequest<GetPlayersResponse>
    {
    }

    public class GetPlayersQueryHandler : IRequestHandler<GetPlayersQuery, GetPlayersResponse>
    {
        private readonly DatabaseContext _context;

        public GetPlayersQueryHandler(DatabaseContext context)
        {
            _context = context;
        }

        public Task<GetPlayersResponse> Handle(GetPlayersQuery request, CancellationToken cancellationToken)
        {
            var players = _context.Players;
            return Task.FromResult(new GetPlayersResponse(players.Select(p => new PlayerDTO(p.Id, p.Position, p.FirstName, p.LastName,
                                                                                                  p.Country, p.Age, p.MarketValue))));
        }
    }
}
