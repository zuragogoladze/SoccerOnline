using MediatR;
using SoccerOnlineManager.Infrastructure.Contexts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Application.Queries.User
{
    public class GetUsersQuery : IRequest<GetUsersResponse>
    {
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, GetUsersResponse>
    {
        private readonly DatabaseContext _context;

        public GetUsersQueryHandler(DatabaseContext context)
        {
            _context = context;
        }

        public Task<GetUsersResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = _context.Users;
            return Task.FromResult(new GetUsersResponse(users.Select(u => new UserDTO(u.Id, u.Email))));
        }
    }
}
