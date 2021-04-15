using System;

namespace SoccerOnlineManager.Application.Queries.Transfer
{
    public class TransferDTO
    {
        public Guid Id { get; set; }

        public string PlayerName { get; set; }

        public string Country { get; set; }

        public decimal Price { get; set; }

        public string TeamName { get; set; }

        public TransferDTO(Guid id, string firstName, string lastName, string country, decimal price, string teamName)
        {
            Id = id;
            PlayerName = string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName) ? null : $"{firstName} {lastName}";
            Country = country;
            Price = price;
            TeamName = teamName;
        }

        // For deserialization
        public TransferDTO()
        { }
    }
}
