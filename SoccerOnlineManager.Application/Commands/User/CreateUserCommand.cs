using MediatR;
using SoccerOnlineManager.Application.Commands.Team;
using SoccerOnlineManager.Application.Exceptions;
using SoccerOnlineManager.Application.Helpers;
using SoccerOnlineManager.Infrastructure.Contexts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Application.Commands.User
{
    public class CreateUserCommand : IRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
    {
        private readonly DatabaseContext _context;
        private readonly IMediator _mediator;

        public CreateUserCommandHandler(DatabaseContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            if (_context.Users.Any(x => x.Email == command.Email))
                throw new ApiException(ExceptionCodes.EmailInUse);

            byte[] hash, salt;
            HashHelper.CreatePasswordHash(command.Password, out hash, out salt);

            var user = new Infrastructure.Entities.User
            {
                Email = command.Email,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Send(new CreateTeamCommand { UserId = user.Id });

            return Unit.Value;
        }
    }
}
