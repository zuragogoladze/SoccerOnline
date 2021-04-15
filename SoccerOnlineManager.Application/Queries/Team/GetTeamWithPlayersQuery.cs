using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoccerOnlineManager.Application.Exceptions;
using SoccerOnlineManager.Application.Queries.Player;
using SoccerOnlineManager.Infrastructure.Contexts;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Application.Queries.Team
{
    public class GetTeamWithPlayersQuery : IRequest<GetTeamWithPlayersResponse>
    {
        [FromRoute(Name = "id")]
        public Guid TeamId { get; set; }
    }

    public class GetTeamWithPlayersQueryHandler : IRequestHandler<GetTeamWithPlayersQuery, GetTeamWithPlayersResponse>
    {
        private readonly DatabaseContext _context;

        public GetTeamWithPlayersQueryHandler(DatabaseContext context)
        {
            _context = context;
        }

        public Task<GetTeamWithPlayersResponse> Handle(GetTeamWithPlayersQuery query, CancellationToken cancellationToken)
        {
            var team = _context.Teams
                .Include(t => t.Players)
                .FirstOrDefault(t => t.UserId == query.TeamId);

            if (team == null)
                throw new ApiException(HttpStatusCode.NotFound);

            var result = new GetTeamWithPlayersResponse(team.UserId, team.Name, team.Country, team.TransferBudget,
                                                        team.Players.Select(p => new PlayerDTO(p.Id, p.Position, p.FirstName, p.LastName,
                                                                                               p.Country, p.Age, p.MarketValue)));

            return Task.FromResult(result);
        }
    }
}
