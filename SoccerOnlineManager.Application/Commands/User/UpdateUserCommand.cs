using MediatR;
using SoccerOnlineManager.Application.Exceptions;
using SoccerOnlineManager.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Application.Commands.User
{
    public class UpdateUserCommand : IRequest
    {
        public Guid Id { get; private set; }

        public string Email { get; set; }

        public UpdateUserCommand(Guid id, string email)
        {
            Id = id;
            Email = email;
        }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly DatabaseContext _context;

        public UpdateUserCommandHandler(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var user = _context.Users.Find(command.Id);

            if (user == null)
                throw new KeyNotFoundException();

            var emailInUse = _context.Users.Any(u => u.Email == command.Email);

            if (emailInUse)
                throw new ApiException(ExceptionCodes.EmailInUse);

            user.Email = command.Email;
            _context.Update(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
