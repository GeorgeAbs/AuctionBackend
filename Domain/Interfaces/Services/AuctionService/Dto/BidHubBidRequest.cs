namespace Domain.Interfaces.Services.AuctionService.Dto
{
    public class BidHubBidRequest
    {
        public Guid Id { get; }

        public Guid BidOwnerId { get; }

        public float BidAmount { get; }

        public BidHubBidRequest(Guid id, Guid bidOwnerId, float bidAmount)
        {
            Id = id;
            BidOwnerId = bidOwnerId;
            BidAmount = bidAmount;
        }
    }
}
