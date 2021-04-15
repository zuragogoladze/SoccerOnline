using System.Collections.Generic;

namespace SoccerOnlineManager.Application.Queries.User
{
    public class GetUsersResponse
    {
        public IEnumerable<UserDTO> Users { get; set; }

        public GetUsersResponse(IEnumerable<UserDTO> users)
        {
            Users = users;
        }
    }
}
