using System.Collections.Generic;

namespace SoccerOnlineManager.Application.Queries.Player
{
    public class GetPlayersResponse
    {
        public IEnumerable<PlayerDTO> Players { get; set; }

        public GetPlayersResponse(IEnumerable<PlayerDTO> players)
        {
            Players = players;
        }
    }
}
