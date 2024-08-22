using Domain.CoreEnums;

namespace Domain.Entities.Orders
{
    public abstract class OrderBase : EntityBase 
    {
        public string OrderIdentifier { get; private set; }

        public Guid ItemId { get; private set; } !!! items no item

        public Guid SellerId { get; private set; } !!!

        public Guid CustomerId { get; private set; }

        public float ItemPrice { get; private set; } !!! many prices
            !!! mb OrderItem?

        public CoreEnums.Enums.PaymentMethod SelectedPaymentMethod { get; private set; }

        private OrderBase() { }

        public OrderBase(string orderIdentifier, Guid itemId, Guid sellerId, Guid customerId, float itemPrice, Enums.PaymentMethod selectedPaymentMethod)
        {
            OrderIdentifier = orderIdentifier;
            ItemId = itemId;
            SellerId = sellerId;
            CustomerId = customerId;
            ItemPrice = itemPrice;
            SelectedPaymentMethod = selectedPaymentMethod;
        }
    }
}
