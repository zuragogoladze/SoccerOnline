using MediatR;
using SoccerOnlineManager.Infrastructure.Contexts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Application.Queries.Team
{
    public class GetTeamsQuery : IRequest<GetTeamsResponse>
    {
    }

    public class GetTeamsQueryHandler : IRequestHandler<GetTeamsQuery, GetTeamsResponse>
    {
        private readonly DatabaseContext _context;

        public GetTeamsQueryHandler(DatabaseContext context)
        {
            _context = context;
        }

        public Task<GetTeamsResponse> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
        {
            var teams = _context.Teams;
            return Task.FromResult(new GetTeamsResponse(teams.Select(t => new TeamDTO(t.UserId, t.Name, t.Country, t.TransferBudget))));
        }
    }
}
