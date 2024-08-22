using Domain.CoreEnums;
using Domain.Entities.Deliveries;

namespace Domain.Entities.Orders
{
    public abstract class DeliveryTypeOrder : OrderBase
    {
        public float DeliveryPrice { get; private set; }

        public float TotalPrice { get; set; }

        public int Quantity { get; private set; }

        public Enums.DeliveryOrderStatus Status { get; protected set; }

        public int DaysForShipment { get; private set; }

        public Delivery Delivery { get; private set; }

        protected DeliveryTypeOrder(string orderIdentifier, Guid itemId, Guid sellerId, Guid customerId, float itemPrice, int quantity, Enums.PaymentMethod selectedPaymentMethod, int DaysForShipment)
            : base(orderIdentifier, itemId, sellerId, customerId, itemPrice, selectedPaymentMethod)
        {

        }

        public abstract void ChangeStatus(Enums.DeliveryOrderStatus status);




    }
}
