using static Domain.CoreEnums.Enums;

namespace Domain.Interfaces.Services.CatalogPaginationService.Dto
{
    public class ItemTradingPaginationResponse
    {
        public Guid Id { get; }

        public string Title { get; }

        public float MinPrice { get; }

        public float MaxPrice { get; }

        public DateTime AuctionEndingTime { get; }

        public SellingTypes SellingType { get; }

        public string? ImageUrl { get; set; }

        public DesignPromotionType DesignPromotionType { get; set; }

        public ItemTradingPaginationResponse(Guid id, string title, float minPrice, float maxPrice, DateTime auctionEndingTime, SellingTypes sellingType, DesignPromotionType designPromotionType)
        {
            Id = id;
            Title = title;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            AuctionEndingTime = auctionEndingTime;
            SellingType = sellingType;
            DesignPromotionType = designPromotionType;
        }
    }
}
