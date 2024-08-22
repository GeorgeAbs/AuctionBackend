namespace Domain.CoreEnums
{
    public class Enums
    {
        public enum ModerationResults
        {
            Denied,
            Approved,
            ForManualModeration,
            ItemIsNull,
            Error
        }

        public enum ItemTradingStatus
        {
            AllStatuses,
            Template,
            DisapprovedByModerator,
            Moderation,
            Published,
            LockedByAuction,
            AuctionIsEnded
        }

        public enum DeliveryOrderStatus
        {
            PendingAuctionItemPayment,
            PendingBasketItemPayment,
            PendingSellerConfirmation,
        }

        public enum NonDeliveryOrderStatus
        {
            PendingAuctionItemPayment,
            PendingBasketItemPayment,
            PendingSellerConfirmation,
        }

        public enum ChatStatuses
        {
            Opened,
            Closed
        }

        public enum UserStatuses
        {
            Created,
            Activated,
            Deleted,
            Blocked,
            Error
        }

        public enum SellingTypes
        {
            Standard,
            Auction,
            /// <summary>
            /// Using for filtration, do not use it for DB
            /// </summary>
            All
        }

        public enum MethodResults
        {
            Ok,
            Conflict,
            NotFound,
            Unauthorized
        }

        public enum ItemPropertyValidationType
        {
            ByRulesForUserTypedText,
            ByStaticList//list of property value from db (or seed?)
        }

        public enum ItemsFreelanceTypes
        {

        }

        public enum AttachmentTypes
        {
            Document,
            Video,
            Picture
        }

        public enum CatalogPropertyTypes
        {
            String,
            Int,
            Float,
            Bool
        }

        public enum SortingMethods
        {
            PriceAsc,
            PriceDesc,
            DateAsc,
            DateDesc,
        }

        public enum ReviewCommentsTypes
        {
            My,
            OnMe
        }

        public enum PaymentMethod
        {
            SafeDeal
        }

        /// <summary>
        /// Origin: isn't resized, Big - slightly resizes, Small - full resizes
        /// </summary>
        public enum ImagePurpose
        {
            OriginImage,
            BigImage,
            SmallImage,
        }

        public enum AuctionSlotStatus
        {
            Created,
            Started,
            EndedWithBids,
            EndedWithoutBids,
            OrderIsCreated
        }

        public enum PaymentPurpose
        {
            ForAuctionSlot,
            ForBasketItem,
            ForFreelance

        }

        public enum DeliveryMethod
        {
            Boxberry,
            PostOfRussia
        }

        public enum DesignPromotionType
        {
            No,
            FirstType,
            SecondType,
            ThirdType
        }

        public enum BannerType
        {
            FistPageBanner
        }
    }
}
