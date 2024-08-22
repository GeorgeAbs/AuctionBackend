using Domain.Entities.AuctionSlots;

namespace Domain.Entities.Images
{
    public class ItemTradingSlotImage : EntityImageBase<ItemTradingAuctionSlot>
    {
        private ItemTradingSlotImage() { }

        public ItemTradingSlotImage(ItemTradingAuctionSlot ownerEntity, string bigImagePath, string smallImagePath) : base(ownerEntity, bigImagePath, smallImagePath)
        {
            ownerEntity.Images.Add(this);
        }
    }
}
