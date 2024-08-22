namespace Application
{
    public static class Settings
    {
        #region folders/files paths
        public const string FilesRootDir = "/files";

        public static readonly string RootFilesPath = Directory.GetParent(Directory.GetCurrentDirectory()) == null ?
            $"{Directory.GetCurrentDirectory()}{FilesRootDir}" : 
            $"{Directory.GetParent(Directory.GetCurrentDirectory())!.FullName}{FilesRootDir}";

        public static readonly string RootFolderForFiles = Directory.GetParent(Directory.GetCurrentDirectory()) == null ?
            $"{Directory.GetCurrentDirectory()}" :
            $"{Directory.GetParent(Directory.GetCurrentDirectory())!.FullName}";

        private static readonly string ImagesFullPath = $"{RootFilesPath}/images";
        private static readonly string ImagesPublicPath = $"{FilesRootDir}/images";

        public static readonly string ImagesOriginFullPath = $"{ImagesFullPath}/o";
        public static readonly string ImagesOriginPublicPath = $"{ImagesPublicPath}/o";
        public static readonly string ImagesBigFullPath = $"{ImagesFullPath}/b";
        public static readonly string ImagesBigPublicPath = $"{ImagesPublicPath}/b";
        public static readonly string ImagesSmallFullPath = $"{ImagesFullPath}/s";
        public static readonly string ImagesSmallPublicPath = $"{ImagesPublicPath}/s";
        #endregion

        public const int HISTORY_PAGE_SIZE = 20;

        public const int BIG_IMAGES_WIDTH_OR_HEIGHT = 1080;
        public const int SMALL_IMAGES_WIDTH_OR_HEIGHT = 600;

        public const int REFRESH_TOKEN_TIME_EXPIRE_DAYS = 7;
        public const int PWD_RESET_CODE_LIFETIME_MINUTES = 5;

        public const int AUCTION_BG_SERVISE_REPEAT_TIME_MS = 30000;
        public const int AUCTION_PENDING_ORDER_PAYMENT_BG_SERVISE_REPEAT_TIME_MS = 30000;
        public const int BASKET_PENDING_ORDER_PAYMENT_BG_SERVISE_REPEAT_TIME_MS = 30000;
        public const int AUCTION_PENDING_ORDER_FORMING_BG_SERVISE_REPEAT_TIME_MS = 30000;
        public const int PENDING_SELLER_ORDER_CONFIRMATION_BG_SERVISE_REPEAT_TIME_MS = 30000;

        public const int TIME_FOR_ORDER_CREATION_FOR_ITEM_TRADING_AUCTION_SLOT_HOURS = 72;

    }
}
