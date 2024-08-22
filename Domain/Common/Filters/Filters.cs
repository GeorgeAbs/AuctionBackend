using System.Collections.ObjectModel;
using static Domain.CoreEnums.Enums;

namespace Domain.Common.Filters
{
    public static class Filters
    {
        /// <summary>
        /// list of sorting methods (price and date)
        /// </summary>
        public static ReadOnlyCollection<SortingMethods> SortingPriceDateList { get; } = new(
            new List<SortingMethods>()
            {
                SortingMethods.DateAsc,
                SortingMethods.DateDesc,
                SortingMethods.PriceAsc,
                SortingMethods.PriceDesc
            }
        );

        /// <summary>
        /// list of sorting methods (price and date)
        /// </summary>
        public static ReadOnlyCollection<SortingItem> SortingPriceDateFilter { get; } = new(
            new List<SortingItem>()
            {
                new (SortingMethods.DateAsc, "Сначала старые"),
                new (SortingMethods.DateDesc, "Сначала новые"),
                new (SortingMethods.PriceAsc, "Сначала дешевые"),
                new (SortingMethods.PriceDesc, "Сначала дорогие")
            }
        );

        /// <summary>
        /// list of sorting methods (only date)
        /// </summary>
        public static ReadOnlyCollection<SortingMethods> SortingDateFilter { get; } = new(
            new List<SortingMethods>()
            {
                SortingMethods.DateAsc,
                SortingMethods.DateDesc
            }
        );

        /// <summary>
        /// list of sorting methods (only date)
        /// </summary>
        public static ReadOnlyCollection<ReviewCommentsTypes> ReviewCommentsFilter { get; } = new(
            new List<ReviewCommentsTypes>()
            {
                ReviewCommentsTypes.My,
                ReviewCommentsTypes.OnMe
            }
        );

        #region sellingType
        private static List<SellingTypes> _sellingTypesChecking = new()
        {
            SellingTypes.Standard,
            SellingTypes.Auction
        };

       
        /// <summary>
        /// statuses for check in service
        /// </summary>
        public static ReadOnlyCollection<SellingTypes> SellingTypesFilterForChecking { get; } = new(_sellingTypesChecking);


        /// <summary>
        /// statuses for sending to UI. List for checking + all statuses
        /// </summary>
        private static List<SellingTypes> _sellingTypesUI = new(_sellingTypesChecking)
        {
            SellingTypes.All
        };

        /// <summary>
        /// list of selling types
        /// </summary>
        public static ReadOnlyCollection<SellingTypeFilterItem> SellingTypeFilter { get; } = new(
            new List<SellingTypeFilterItem>()
            {
                new (SellingTypes.Standard, "Только стандартная продажа"),
                new (SellingTypes.Auction, "Только аукционы"),
                new (SellingTypes.All, "Любой тип продажи")
            }
        );
        #endregion

        #region purchasing
        /// <summary>
        /// statuses for check in service
        /// </summary>
        private static List<ItemTradingStatus> _itemTradingStatusesPurchasingCheching = new()
        {
            ItemTradingStatus.Published
        };

        /// <summary>
        /// statuses for check in service
        /// </summary>
        public static ReadOnlyCollection<ItemTradingStatus> ItemTradingStatusesPurchasingFilterForChecking { get; } = new(_itemTradingStatusesPurchasingCheching);


        /// <summary>
        /// statuses for sending to UI. List for checking + all statuses
        /// </summary>
        private static List<ItemTradingStatus> _itemtradingStatusesPurchasingUI = new(_itemTradingStatusesPurchasingCheching)
        {
            ItemTradingStatus.AllStatuses,
            ItemTradingStatus.Template
        };

        /// <summary>
        /// statuses for sending to UI
        /// </summary>
        public static ReadOnlyCollection<ItemTradingStatus> ItemTradingStatusesMyOrdersFilterForUI { get; } = new(_itemtradingStatusesPurchasingUI);
        #endregion

        #region selling
        /// <summary>
        /// statuses for check in service
        /// </summary>
        private static List<ItemTradingStatus> _itemTradingStatusesSellingChecking = new()
        {
            ItemTradingStatus.Template,
            ItemTradingStatus.Moderation,
            ItemTradingStatus.DisapprovedByModerator,
            ItemTradingStatus.Published
        };

        /// <summary>
        /// statuses for check in service
        /// </summary>
        public static ReadOnlyCollection<ItemTradingStatus> ItemTradingStatusesSellingFilterForChecking { get; } = new(_itemTradingStatusesSellingChecking);


        /// <summary>
        /// statuses for sending to UI. List for checking + all statuses
        /// </summary>
        private static List<ItemTradingStatus> _itemTradingStatusesSellingUI = new(_itemTradingStatusesSellingChecking)
        {
            ItemTradingStatus.AllStatuses
        };

        /// <summary>
        /// statuses for sending to UI
        /// </summary>
        public static ReadOnlyCollection<ItemTradingStatus> ItemTradingStatusesMyItemsFilterForUI { get; } = new(_itemTradingStatusesSellingUI);
        #endregion

        #region auction
        /// <summary>
        /// statuses for check in service
        /// </summary>
        private static List<ItemTradingStatus> _itemTradingStatusesAuctionCheching = new()
        {
            ItemTradingStatus.Published
        };

        /// <summary>
        /// statuses for check in service
        /// </summary>
        public static ReadOnlyCollection<ItemTradingStatus> ItemTradingStatusesAuctionFilterForChecking { get; } = new(_itemTradingStatusesAuctionCheching);


        /// <summary>
        /// statuses for sending to UI. List for checking + all statuses
        /// </summary>
        private static List<ItemTradingStatus> _itemTradingStatusesAuctionUI = new(_itemTradingStatusesAuctionCheching)
        {
            ItemTradingStatus.AllStatuses,
        };

        /// <summary>
        /// statuses for sending to UI
        /// </summary>
        public static ReadOnlyCollection<ItemTradingStatus> ItemTradingStatusesAuctionFilterForUI { get; } = new(_itemTradingStatusesAuctionUI);
        #endregion
    }
}
