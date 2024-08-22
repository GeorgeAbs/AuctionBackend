using Domain.CoreEnums;
using Domain.Entities.Addresses;
using Domain.Entities.Images;
using Domain.Entities.Orders.History;
using Domain.Entities.PaymentMethods;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.Orders
{
    public class ItemTradingOrder : Order
    {
        private List<UserAddress> _shipmentAddresses = [];
        [BackingField(nameof(_shipmentAddresses))]
        public IEnumerable<UserAddress> ShipmentAddresses { get { return _shipmentAddresses; } }

        public UserAddress SelectedAddress { get; private set; }

        private List<ItemTradingOrderHistoryNote> _historyNotes = [];
        [BackingField(nameof(_historyNotes))]
        public IEnumerable<ItemTradingOrderHistoryNote> HistoryNotes { get { return _historyNotes; } }

        public ItemTradingOrder(string orderIdentifier,
            string orderTitle,
            Guid itemId,
            Guid sellerId,
            Guid customerId,
            string itemInfo,
            float goodsPrice,
            List<PaymentMethod> paymentMethods,
            OrderImage itemImage,
            int daysForShipment,
            CoreEnums.Enums.PaymentPurpose paymentPurpose,
            float pricePerPcs,
            int quantity = 1) : base(orderIdentifier,
                orderTitle,
                itemId,
                sellerId,
                customerId,
                itemInfo,
                goodsPrice,
                paymentMethods,
                itemImage,
                daysForShipment,
                paymentPurpose)
        {
            PricePerPcs = pricePerPcs;
            Quantity = quantity;
        }

        public override void ChangeStatus(Enums.OrderStatus status)
        {
            if (status != Status) 
            {
                Status = status;

                _historyNotes.Add(new ItemTradingOrderHistoryNote(this, status));
            }
        }
    }
}
