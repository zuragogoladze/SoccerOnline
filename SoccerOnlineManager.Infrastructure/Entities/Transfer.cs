using SoccerOnlineManager.Infrastructure.Enums;
using System;

namespace SoccerOnlineManager.Infrastructure.Entities
{
    public class Transfer
    {
        public Guid Id { get; set; }

        public decimal Price { get; set; }

        public TransferStatus Status { get; set; }

        /// <summary>
        /// For history
        /// </summary>
        public Guid FromTeam { get; set; }

        /// <summary>
        /// For history
        /// </summary>
        public Guid? ToTeam { get; set; }

        public Guid PlayerId { get; set; }
        public Player Player { get; set; }
    }
}
