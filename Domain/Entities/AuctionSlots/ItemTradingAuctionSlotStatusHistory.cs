using Domain.Entities.Bid;
using static Domain.CoreEnums.Enums;

namespace Domain.Entities.AuctionSlots
{
    public class ItemTradingAuctionSlotStatusHistory : BaseAuctionSlotStatusHistory<ItemTradingAuctionSlot,ItemTradingBid>
    {
        private ItemTradingAuctionSlotStatusHistory() { }

        public ItemTradingAuctionSlotStatusHistory(ItemTradingAuctionSlot slot, AuctionSlotStatus status) :base(slot, status)
        {

        }
    }
}
