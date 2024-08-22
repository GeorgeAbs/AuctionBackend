namespace Domain.Entities.Orders.Preorders
{
    public class OrderItem : EntityBase
    {
        public Guid CustomerId { get; private set; }

        public Guid SellerId { get; private set; }

        public Guid ItemId { get; private set; }

        public float ItemPrice { get; private set; }

        public int Quantity { get; private set; }


    }
}
