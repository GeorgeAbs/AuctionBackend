using Domain.Entities.Items.ItemTrading;

namespace Domain.Entities.ModerationDisappReason
{
    public class ItemTradingModerationDisappReason : ModerationDisappReason<ItemTrading>
    {
        private ItemTradingModerationDisappReason() { }

        public ItemTradingModerationDisappReason(ItemTrading item, string reason, Guid moderatorId) : base(item, reason, moderatorId) { } 
    }
}
