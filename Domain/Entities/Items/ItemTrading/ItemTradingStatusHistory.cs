using static Domain.CoreEnums.Enums;

namespace Domain.Entities.Items.ItemTrading
{
    public class ItemTradingStatusHistory : EntityBase
    {
        public ItemTrading Item { get; private set; }

        public ItemTradingStatus Status { get; private set; }

        private ItemTradingStatusHistory() { }

        public ItemTradingStatusHistory(ItemTrading item, ItemTradingStatus currentStatus)
        {
            Item = item;
            Status = currentStatus;
        }
    }
}
