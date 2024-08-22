namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto.AuctionItem
{
    public class ItemTradingAuctionSlotInfoResponse
    {
        public string Title { get; private set; } = string.Empty;

        public int SlotNumber { get; private set; } = 1;

        public string Description { get; private set; } = string.Empty;

        public int Price { get; private set; }

        public int MinimumBid { get; private set; }

        public int BlitzPrice { get; private set; }

        public IEnumerable<IntPropertyResponse> IntProperties { get; private set; } = [];

        public IEnumerable<FloatPropertyResponse> FloatProperties { get; private set; } = [];

        public IEnumerable<StringPropertyResponse> StringProperties { get; private set; } = [];

        public IEnumerable<BoolPropertyResponse> BoolProperties { get; private set; } = [];

        public string? ImageURL { get; private set; } = string.Empty;

        public ItemTradingAuctionSlotInfoResponse(string title, 
            int slotNumber, 
            string description, 
            float price, 
            float minimumBid, 
            float blitzPrice, 
            IEnumerable<IntPropertyResponse> intProperties, 
            IEnumerable<FloatPropertyResponse> floatProperties, 
            IEnumerable<StringPropertyResponse> stringProperties, 
            IEnumerable<BoolPropertyResponse> boolProperties, 
            string? imageUrl)
        {
            Title = title;
            SlotNumber = slotNumber;
            Description = description;
            Price = (int)price;
            MinimumBid = (int)minimumBid;
            BlitzPrice = (int)blitzPrice;
            IntProperties = intProperties;
            FloatProperties = floatProperties;
            StringProperties = stringProperties;
            BoolProperties = boolProperties;
            ImageURL = imageUrl;
        }
    }
}
