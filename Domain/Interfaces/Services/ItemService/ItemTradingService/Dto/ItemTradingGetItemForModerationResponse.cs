namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto
{
    public class ItemTradingGetItemForModerationResponse
    {
        public ItemTradingGetItemInfoResponse ItemInfo { get; private set; }

        public ItemTradingGetPaymentInfoResponse PaymentInfo { get; private set; }

        public ItemTradingGetDeliveryInfoResponse DeliveryInfo { get; private set; }

        public ItemTradingGetItemForModerationResponse(ItemTradingGetItemInfoResponse itemInfo,
            ItemTradingGetPaymentInfoResponse paymentInfo,
            ItemTradingGetDeliveryInfoResponse deliveryInfo)
        {
            ItemInfo = itemInfo;
            PaymentInfo = paymentInfo;
            DeliveryInfo = deliveryInfo;
        }
    }
}
