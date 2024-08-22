namespace Domain.Interfaces.Services.BasketService.Dto
{
    public class BasketSellerGroup
    {
        public float TotalPrice { get; set; }

        public string SellerName { get; private set; }

        public string UserName { get; private set; }

        public IEnumerable<BasketSendingTermsGroup> BasketSendingTermsGroups { get; set; } = [];

        public BasketSellerGroup(string sellerName, string userName)
        {
            SellerName = sellerName;
            UserName = userName;
        }
    }
}
