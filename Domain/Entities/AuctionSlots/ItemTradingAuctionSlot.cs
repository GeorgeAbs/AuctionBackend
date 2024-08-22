using Domain.BackendResponses;
using Domain.Entities.Bid;
using Domain.Entities.Catalog;
using Domain.Entities.Catalog.CatalogProperty;
using Domain.Entities.Images;
using Domain.Interfaces;
using static Domain.CoreEnums.Enums;

namespace Domain.Entities.AuctionSlots
{
    public class ItemTradingAuctionSlot : BaseAuctionSlot<ItemTradingBid, ItemTradingAuctionSlotStatusHistory>, IItemTradingCatalogItem
    {
        public CatalogCategory CatalogCategory { get; private set; }

        public List<CatalogStringProperty> StringProperties { get; private set; } = [];

        public List<CatalogFloatProperty> FloatProperties { get; private set; } = [];

        public List<CatalogIntProperty> IntProperties { get; private set; } = [];

        public List<CatalogBoolProperty> BoolProperties { get; private set; } = [];

        public List<ItemTradingSlotImage> Images { get; set; } = [];

        private ItemTradingAuctionSlot() { }

        public ItemTradingAuctionSlot(CatalogCategory catalogCategory,
            Guid itemId, string title, string description, float price, float minimumBid, float blitzPrice, DateTime auctionEndingTime, int auctionSlotNum)
            : base(itemId, title, description, price, minimumBid, blitzPrice, auctionEndingTime, auctionSlotNum)
        {
            CatalogCategory = catalogCategory;
        }

        public override void ChangeStatus(AuctionSlotStatus status)
        {
            if (status == Status) return;
            Status = status;
            _statusHistories.Add(new ItemTradingAuctionSlotStatusHistory(this, status));
        }

        public MethodResult SetInfo(string description,
            float price,
            float minimumBid,
            float blitzPrice,
            DateTime auctionEndingTime,
            int auctionSlotNum,
            List<ItemTradingSlotImage> images,
            List<CatalogStringProperty>? stringProperties = null,
            List<CatalogFloatProperty>? floatProperties = null,
            List<CatalogIntProperty>? intProperties = null,
            List<CatalogBoolProperty>? boolProperties = null)
        {
            if (price <= 0 || minimumBid <= 0 || blitzPrice < price + minimumBid || auctionEndingTime <= DateTime.UtcNow) return new MethodResult([], MethodResults.Conflict);

            Description = description ?? "";
            Price = price;
            MinimumBid = minimumBid;
            BlitzPrice = blitzPrice;
            AuctionEndingTime = auctionEndingTime;
            AuctionSlotNum = auctionSlotNum;
            Images = images;

            if (stringProperties is not null) 
            {
                StringProperties.Clear();
                StringProperties.AddRange(stringProperties);
            }

            if (intProperties != null)
            {
                IntProperties.Clear();
                IntProperties.AddRange(intProperties);
            }

            if (floatProperties != null)
            {
                FloatProperties.Clear();
                FloatProperties.AddRange(floatProperties);
            }

            if (boolProperties != null)
            {
                BoolProperties.Clear();
                BoolProperties.AddRange(boolProperties);
            }

            return new MethodResult([], MethodResults.Ok);
        }

        public override void PrepareForDeleting()
        {
            base.PrepareForDeleting();

            //Image = null;
        }
    }
}
