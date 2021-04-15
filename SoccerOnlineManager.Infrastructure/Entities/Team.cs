using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoccerOnlineManager.Infrastructure.Entities
{
    public class Team
    {
        [Key]
        public Guid UserId { get; set; }
        public User User { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public decimal TransferBudget { get; set; }

        public ICollection<Player> Players { get; set; }
    }
}
