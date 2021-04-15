using SoccerOnlineManager.Application.Queries.Player;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoccerOnlineManager.Application.Queries.Team
{
    public class GetTeamWithPlayersResponse
    {
        public Guid TeamId { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public decimal TransferBudget { get; set; }

        public decimal TeamValue { get; set; }

        public IEnumerable<PlayerDTO> Players { get; set; }

        public GetTeamWithPlayersResponse(Guid teamId, string name, string country, decimal transferBudget, IEnumerable<PlayerDTO> players)
        {
            TeamId = teamId;
            Name = name;
            Country = country;
            TransferBudget = transferBudget;
            TeamValue = players.Sum(p => p.MarketValue);
            Players = players;
        }
    }
}
