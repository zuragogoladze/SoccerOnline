using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoccerOnlineManager.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Application.Commands.User
{
    public class DeleteUserCommand : IRequest
    {
        [FromRoute(Name = "id")]
        public Guid Id { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly DatabaseContext _context;

        public DeleteUserCommandHandler(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var user = _context.Users.Find(command.Id);

            if (user == null)
                throw new KeyNotFoundException();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
