namespace Domain.Interfaces.Services.BasketService.Dto
{
    public class BasketSendingTermsGroup
    {
        public int DaysForShipment { get; private set; }

        public float TotalPrice { get; set; }

        public IEnumerable<BasketItemResponse> BasketItems { get;  set; } = [];

        public BasketSendingTermsGroup(int daysForShipment)
        {
            DaysForShipment = daysForShipment;
        }
    }
}
