namespace Domain.Entities.Orders.History
{
    public class ItemTradingOrderHistoryNote : OrderHistoryNote<ItemTradingOrder>
    {
        public ItemTradingOrderHistoryNote(ItemTradingOrder order, CoreEnums.Enums.OrderStatus status) : base(order, status) { }
    }
}
