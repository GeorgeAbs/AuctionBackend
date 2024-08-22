using static Domain.CoreEnums.Enums;

namespace Domain.Interfaces.Services.AuctionService.Dto
{
    public class BidHubSlotRequest
    {
        public Guid Id { get; }

        public DateTime AuctionEndingTime { get; }

        public AuctionSlotStatus Status { get; }

        public BidHubSlotRequest(Guid id,
            DateTime auctionEndingTime,
            AuctionSlotStatus status)
        {
            Id = id;
            AuctionEndingTime = auctionEndingTime;
            Status = status;
        }
    }
}
