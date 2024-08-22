namespace Domain.Entities.Orders.Preorders
{
    public abstract class PreorderBase : EntityBase
    {
        public Guid CustomerId { get; private set; }
        !!! PreorderItems
        public Guid SellerId { get; private set; }

        public Guid ItemId { get; private set; }

        public float ItemPrice { get; private set; }

        private PreorderBase() { }

        public PreorderBase(Guid customerId, Guid sellerId, Guid itemId, float itemPrice)
        {
            CustomerId = customerId;
            SellerId = sellerId;
            ItemId = itemId;
            ItemPrice = itemPrice;
        }
    }
}