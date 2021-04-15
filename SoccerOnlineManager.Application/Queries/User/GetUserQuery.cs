using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoccerOnlineManager.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Application.Queries.User
{
    public class GetUserQuery : IRequest<UserDTO>
    {
        [FromRoute(Name = "id")]
        public Guid Id { get; set; }
    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDTO>
    {
        private readonly DatabaseContext _context;

        public GetUserQueryHandler(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<UserDTO> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == query.Id);

            if (user == null)
                throw new KeyNotFoundException();

            return new UserDTO(user.Id, user.Email);
        }
    }
}
