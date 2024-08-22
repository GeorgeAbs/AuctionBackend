namespace Domain.Interfaces.Services.BasketService.Dto
{
    public class BasketResponse
    {
        public IEnumerable<BasketSellerGroup> BasketSellerGroups { get; private set; }

        public float TotalPrice { get; private set; }

        public BasketResponse(IEnumerable<BasketSellerGroup> basketSellerGroups, float totalPrice)
        {
            BasketSellerGroups = basketSellerGroups;
            TotalPrice = totalPrice;
        }
    }
}
