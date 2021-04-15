﻿using System;

namespace SoccerOnlineManager.Infrastructure.Entities
{
    public class UserRole
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}
