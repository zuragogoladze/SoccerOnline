using System;

namespace SoccerOnlineManager.Application.Commands.User
{
    public class AuthenticateUserResponse
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }

        public AuthenticateUserResponse(Guid id, string email, string token)
        {
            Id = id;
            Email = email;
            Token = token;
        }
    }
}
