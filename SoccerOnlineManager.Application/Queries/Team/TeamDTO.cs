using System;

namespace SoccerOnlineManager.Application.Queries.Team
{
    public class TeamDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public decimal TransferBudget { get; set; }

        public TeamDTO(Guid id, string name, string country, decimal transferBudget)
        {
            Id = id;
            Name = name;
            Country = country;
            TransferBudget = transferBudget;
        }
    }
}
