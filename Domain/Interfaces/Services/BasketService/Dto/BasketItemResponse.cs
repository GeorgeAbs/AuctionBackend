namespace Domain.Interfaces.Services.BasketService.Dto
{
    public class BasketItemResponse
    {
        public Guid ItemId { get; private set; }

        public string ItemTitle { get; private set; }

        public string ImageUrl { get; private set; }

        public int Quantity { get; private set; }

        public float ItemPrice { get; private set; }

        public float ItemsTotalPrice { get; private set; }

        public BasketItemResponse(Guid itemId, string itemTitle, string imageUrl, int quantity, float itemPrice, float itemsTotalPrice)
        {
            ItemId = itemId;
            ItemTitle = itemTitle;
            ImageUrl = imageUrl;
            Quantity = quantity;
            ItemPrice = itemPrice;
            ItemsTotalPrice = itemsTotalPrice;
        }
    }
}
