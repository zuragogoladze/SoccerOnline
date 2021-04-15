using System;
using System.Collections.Generic;

namespace SoccerOnlineManager.Infrastructure.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

        public Team Team { get; set; }
    }
}
