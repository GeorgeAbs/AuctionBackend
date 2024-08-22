using Domain.Interfaces.Services.ItemService.ItemTradingService.Dto.PropertiesForGetItemRequest;

namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto.AuctionItem
{
    public class ItemTradingAuctionSlotInfoGetItemResponse
    {
        public Guid Id { get; private set; }

        public string Title { get; private set; } = string.Empty;

        public int SlotNumber { get; private set; } = 1;

        public string Description { get; private set; } = string.Empty;

        public int Price { get; private set; }

        public int MinimumBid { get; private set; }

        public int BlitzPrice { get; private set; }

        public IEnumerable<IntItemPropertyGetItemResponse> IntProperties { get; private set; } = [];

        public IEnumerable<FloatItemPropertyGetItemResponse> FloatProperties { get; private set; } = [];

        public IEnumerable<StringItemPropertyGetItemResponse> StringProperties { get; private set; } = [];

        public IEnumerable<BoolItemPropertyGetItemResponse> BoolProperties { get; private set; } = [];

        public string? ImageURL { get; private set; } = string.Empty;

        public ItemTradingAuctionSlotInfoGetItemResponse(Guid id,
            string title,
            int slotNumber,
            string description,
            int price,
            int minimumBid,
            int blitzPrice,
            IEnumerable<IntItemPropertyGetItemResponse> intProperties,
            IEnumerable<FloatItemPropertyGetItemResponse> floatProperties,
            IEnumerable<StringItemPropertyGetItemResponse> stringProperties,
            IEnumerable<BoolItemPropertyGetItemResponse> boolProperties,
            string? imageURL)
        {
            Id = id;
            Title = title;
            SlotNumber = slotNumber;
            Description = description;
            Price = price;
            MinimumBid = minimumBid;
            BlitzPrice = blitzPrice;
            IntProperties = intProperties;
            FloatProperties = floatProperties;
            StringProperties = stringProperties;
            BoolProperties = boolProperties;
            ImageURL = imageURL;
        }
    }
}
