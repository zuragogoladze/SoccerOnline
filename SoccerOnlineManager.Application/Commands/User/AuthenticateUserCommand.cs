using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SoccerOnlineManager.Application.Exceptions;
using SoccerOnlineManager.Application.Helpers;
using SoccerOnlineManager.Application.Settings;
using SoccerOnlineManager.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Application.Commands.User
{
    public class AuthenticateUserCommand : IRequest<AuthenticateUserResponse>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, AuthenticateUserResponse>
    {
        private readonly DatabaseContext _context;
        private readonly JWTSettings _jwtSettings;

        public AuthenticateUserCommandHandler(DatabaseContext context, IOptions<JWTSettings> jwtOptions)
        {
            _context = context;
            _jwtSettings = jwtOptions.Value;
        }

        public Task<AuthenticateUserResponse> Handle(AuthenticateUserCommand command, CancellationToken cancellationToken)
        {
            var user = _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).SingleOrDefault(x => x.Email == command.Email);

            if (user == null)
                throw new ApiException(ExceptionCodes.InvalidUserCredentials);

            try
            {
                if (!HashHelper.VerifyPasswordHash(command.Password, user.PasswordHash, user.PasswordSalt))
                    throw new ApiException(ExceptionCodes.InvalidUserCredentials);
            }
            catch (Exception)
            {
                throw new ApiException(ExceptionCodes.InvalidUserCredentials);
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Id.ToString()) };

            if(user.UserRoles != null)
                foreach (var item in user.UserRoles.Select(x => x.Role))
                    claims.Add(new Claim (ClaimTypes.Role, item.Name));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var response = new AuthenticateUserResponse(user.Id, user.Email, tokenHandler.WriteToken(token));

            return Task.FromResult(response);
        }
    }
}
