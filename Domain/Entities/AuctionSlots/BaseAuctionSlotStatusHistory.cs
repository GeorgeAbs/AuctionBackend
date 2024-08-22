using Domain.Interfaces;
using static Domain.CoreEnums.Enums;
namespace Domain.Entities.AuctionSlots
{
    public abstract class BaseAuctionSlotStatusHistory<TSlot, TBid>: EntityBase where TSlot: IAuctionSlot<TBid> where TBid : class
    {
        public TSlot Slot { get; }

        public AuctionSlotStatus Status { get; }

        protected BaseAuctionSlotStatusHistory() { }

        public BaseAuctionSlotStatusHistory(TSlot slot, AuctionSlotStatus status)
        {
            Slot = slot;
            Status = status;
        }
    }
}
