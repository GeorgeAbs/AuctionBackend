namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto
{
    public class ItemTradingGetDeliveryInfoResponse
    {
        public int DaysForShipment { get; private set; }

        public List<string> DeliveryMethods { get; private set; } = [];

        public void SetInfo(int daysForShipment, List<string> deliveryMethods)
        {
            DaysForShipment = daysForShipment;
            DeliveryMethods = deliveryMethods;
        }
    }
}
