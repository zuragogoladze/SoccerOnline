using System.Collections.Generic;

namespace SoccerOnlineManager.Application.Queries.Team
{
    public class GetTeamsResponse
    {
        public IEnumerable<TeamDTO> Teams { get; set; }

        public GetTeamsResponse(IEnumerable<TeamDTO> teams)
        {
            Teams = teams;
        }
    }
}
