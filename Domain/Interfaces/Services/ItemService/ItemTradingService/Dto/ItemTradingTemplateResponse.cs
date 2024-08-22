using Domain.CoreEnums;
using Domain.Interfaces.Services.ItemService.ItemTradingService.Dto.AuctionItem;
using static Domain.CoreEnums.Enums;

namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto
{
    public class ItemTradingTemplateResponse
    {
        public Guid ItemId { get; }

        public string Title { get;}

        public string Description { get; }

        public IEnumerable<string> ImagesURLs { get; }

        public SellingTypes SellingType { get; }

        public IEnumerable<ItemTradingAuctionSlotInfoResponse> Slots { get; private set; } = [];

        public DateTime AuctionEndingTime { get; private set; }

        public int DaysForShipment { get; }

        public float Price { get; }

        public string CatalogSystemName { get; private set; }

        public IEnumerable<IntPropertyResponse> IntProperties { get;}

        public IEnumerable<FloatPropertyResponse> FloatProperties { get; }

        public IEnumerable<StringPropertyResponse> StringProperties { get; }

        public IEnumerable<BoolPropertyResponse> BoolProperties { get; }

        public IEnumerable<AddressItemTemplateResponse> ShipmentAddresses { get; }

        public IEnumerable<Enums.PaymentMethod> PaymentMethods { get; }

        public int Quantity { get; }

        public ItemTradingTemplateResponse(Guid itemId,
            string catalogSystemName,
            string title,
            string description,
            SellingTypes sellingType,
            DateTime auctionEndingTime,
            float price,
            IEnumerable<string> imagesURLs,
            IEnumerable<IntPropertyResponse> intProperties,
            IEnumerable<FloatPropertyResponse> floatProperties,
            IEnumerable<StringPropertyResponse> stringProperties,
            IEnumerable<BoolPropertyResponse> boolProperties,
            IEnumerable<AddressItemTemplateResponse> shipmentAddresses,
            IEnumerable<Enums.PaymentMethod> paymentMethods,
            int quantity,
            int daysForShipment)
        {
            ItemId = itemId;
            CatalogSystemName = catalogSystemName;
            Title = title;
            Description = description;
            SellingType = sellingType;
            AuctionEndingTime = auctionEndingTime;
            Price = price;
            ImagesURLs = imagesURLs;
            IntProperties = intProperties;
            FloatProperties = floatProperties;
            StringProperties = stringProperties;
            BoolProperties = boolProperties;
            ShipmentAddresses = shipmentAddresses;
            PaymentMethods = paymentMethods;
            Quantity = quantity;
            DaysForShipment = daysForShipment;

        }

        public void SetAuctionSlots(IEnumerable<ItemTradingAuctionSlotInfoResponse> slots)
        {
            Slots = slots;
        }
    }
}
