using System.Collections.Generic;

namespace SoccerOnlineManager.Application.Queries.Transfer
{
    public class GetTransfersResponse
    {
        public IEnumerable<TransferDTO> Transfers { get; set; }

        public GetTransfersResponse(IEnumerable<TransferDTO> transfers)
        {
            Transfers = transfers;
        }
    }
}
