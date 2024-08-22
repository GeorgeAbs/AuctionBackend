using Domain.Entities.AuctionSlots;

namespace Domain.Entities.Bid
{
    public class ItemTradingBid :  BaseBid<ItemTradingAuctionSlot>
    {
        private ItemTradingBid() { }
        public ItemTradingBid(Guid bidOwnerId, float bidAmount, ItemTradingAuctionSlot bidSlot) 
            : base(bidOwnerId, bidAmount, bidSlot)
        {

        }
    }
}
