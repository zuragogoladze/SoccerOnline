using SoccerOnlineManager.Infrastructure.Enums;
using System;

namespace SoccerOnlineManager.Application.Queries.Player
{
    public class PlayerDTO
    {
        public Guid Id { get; set; }

        public Position Position { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Country { get; set; }

        public int Age { get; set; }

        public decimal MarketValue { get; set; }

        public PlayerDTO(Guid id, Position position, string firstName, string lastName, string country, int age, decimal marketValue)
        {
            Id = id;
            Position = position;
            FirstName = firstName;
            LastName = lastName;
            Country = country;
            Age = age;
            MarketValue = marketValue;
        }
    }
}
