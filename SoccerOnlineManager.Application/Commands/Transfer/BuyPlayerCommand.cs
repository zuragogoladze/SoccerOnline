using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SoccerOnlineManager.Application.Exceptions;
using SoccerOnlineManager.Application.Settings;
using SoccerOnlineManager.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Application.Commands.Transfer
{
    public class BuyPlayerCommand : IRequest
    {
        public Guid Id { get; private set; }

        public Guid BuyerId { get; private set; }

        public BuyPlayerCommand(Guid id, Guid buyerId)
        {
            Id = id;
            BuyerId = buyerId;
        }
    }

    public class BuyPlayerCommandHandler : IRequestHandler<BuyPlayerCommand>
    {
        private readonly DatabaseContext _context;
        private readonly GameSettings _gameSettings;

        public BuyPlayerCommandHandler(DatabaseContext context, IOptions<GameSettings> gameOptions)
        {
            _context = context;
            _gameSettings = gameOptions.Value;
        }

        public async Task<Unit> Handle(BuyPlayerCommand command, CancellationToken cancellationToken)
        {
            var transfer = _context.Transfers
                .Include(t => t.Player)
                    .ThenInclude(p => p.Team)
                .FirstOrDefault(t => t.Id == command.Id);

            if (transfer == null)
                throw new KeyNotFoundException();

            if (transfer.Status == Infrastructure.Enums.TransferStatus.Sold)
                throw new ApiException(ExceptionCodes.PlayerSold);

            var toTeam = _context.Teams.Find(command.BuyerId);

            if (toTeam == null)
                throw new ApiException(ExceptionCodes.NoTeam);

            if (transfer.FromTeam == toTeam.UserId)
                throw new ApiException(ExceptionCodes.YourPlayer);

            if (toTeam.TransferBudget < transfer.Price)
                throw new ApiException(ExceptionCodes.InsufficientFunds);

            var random = new Random();
            var playerPriceIncreasePercent = random.Next(_gameSettings.MinPriceIncrease, _gameSettings.MaxPriceIncrease);
            var playerValue = transfer.Player.MarketValue;

            toTeam.TransferBudget -= transfer.Price;
            transfer.Player.Team.TransferBudget += transfer.Price;
            transfer.Player.MarketValue = playerValue + playerValue * playerPriceIncreasePercent / 100;
            transfer.Status = Infrastructure.Enums.TransferStatus.Sold;
            transfer.Player.TeamId = toTeam.UserId;

            _context.Update(transfer);
            _context.Update(toTeam);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
