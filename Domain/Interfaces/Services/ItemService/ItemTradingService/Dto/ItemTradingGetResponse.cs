namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto
{
    public class ItemTradingGetResponse
    {
        public ItemTradingGetItemInfoResponse ItemInfo {  get; private set; }

        public ItemTradingGetPaymentInfoResponse PaymentInfo { get; private set; }

        public ItemTradingGetDeliveryInfoResponse DeliveryInfo { get; private set; }

        public ItemTradingGetUserInfoResponse UserInfo { get; private set; }

        public ItemTradingGetResponse(ItemTradingGetItemInfoResponse itemInfo,
            ItemTradingGetPaymentInfoResponse paymentInfo,
            ItemTradingGetUserInfoResponse userInfo,
            ItemTradingGetDeliveryInfoResponse deliveryInfo)
        {
            ItemInfo = itemInfo;
            PaymentInfo = paymentInfo;
            UserInfo = userInfo;
            DeliveryInfo = deliveryInfo;
        }
    }
}
