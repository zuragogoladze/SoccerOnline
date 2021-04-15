using MediatR;
using SoccerOnlineManager.Infrastructure.Contexts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Application.Queries.Transfer
{
    public class GetTransfersQuery : IRequest<GetTransfersResponse>
    {
        public string Country { get; set; }

        public string TeamName { get; set; }

        public string PlayerName { get; set; }

        public decimal? FromValue { get; set; }

        public decimal? ToValue { get; set; }
    }

    public class GetTransfersQueryHandler : IRequestHandler<GetTransfersQuery, GetTransfersResponse>
    {
        private readonly DatabaseContext _context;

        public GetTransfersQueryHandler(DatabaseContext context)
        {
            _context = context;
        }

        public Task<GetTransfersResponse> Handle(GetTransfersQuery query, CancellationToken cancellationToken)
        {
            var transfers = _context.Transfers
                .Where(t => t.Status == Infrastructure.Enums.TransferStatus.Active &&
                           (query.Country == null || t.Player.Country.Contains(query.Country)) &&
                           (query.TeamName == null || t.Player.Team.Name.Contains(query.TeamName)) &&
                           (query.PlayerName == null || (t.Player.FirstName + t.Player.LastName).Contains(query.PlayerName)) &&
                           (query.FromValue == null || t.Price >= query.FromValue) &&
                           (query.ToValue == null || t.Price <= query.ToValue));

            var result = new GetTransfersResponse(transfers.Select(t => new TransferDTO(t.Id, t.Player.FirstName, t.Player.LastName,
                                                                                        t.Player.Country, t.Price, t.Player.Team.Name)));

            return Task.FromResult(result);
        }
    }
}
