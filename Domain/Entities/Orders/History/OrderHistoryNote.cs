namespace Domain.Entities.Orders.History
{
    public abstract class OrderHistoryNote<TOrder> : EntityBase where TOrder : Order
    {
        public TOrder Order { get; private set; }

        public CoreEnums.Enums.OrderStatus Status { get; private set; }

        private OrderHistoryNote() { }

        public OrderHistoryNote(TOrder order, CoreEnums.Enums.OrderStatus status)
        {
            Order = order;
            Status = status;
        }
    }
}
