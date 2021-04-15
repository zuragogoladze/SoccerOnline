using SoccerOnlineManager.Infrastructure.Enums;
using System;
using System.Collections.Generic;

namespace SoccerOnlineManager.Infrastructure.Entities
{
    public class Player
    {
        public Guid Id { get; set; }

        public Position Position { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Country { get; set; }

        public int Age { get; set; }

        public decimal MarketValue { get; set; }

        public Guid TeamId { get; set; }
        public Team Team { get; set; }

        public ICollection<Transfer> Transfers { get; set; }
    }
}
