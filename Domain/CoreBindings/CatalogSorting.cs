

namespace Domain.CoreBindings
{
    public static class CatalogSorting
    {
        public const string CREATION_DATE = "creation_date";
        public const string PRICE = "price";
        public const string SELLER_RATING = "seller_rating";
        public const string AUCTION_REMAINING_TIME = "auction_remaining_time";

        public const string ASCENDING = "ascending";
        public const string DESCENDING = "descending";
        public const string NONE = "none";

        public static Dictionary<string, string> CatalogSortingBinding { get; } = new Dictionary<string, string>()
            {
                {SELLER_RATING, "По рейтингу продавца"},
                {PRICE, "По цене"},
                {AUCTION_REMAINING_TIME, "По окончанию аукциона"},
                {CREATION_DATE, "По новизне"}
            };

        public static Dictionary<bool, string> CatalogSortingDirectionBinding { get; } = new Dictionary<bool, string>()
            {
                {true, "По убыванию"},
                {false, "По возрастанию"}
            };
    }
}
