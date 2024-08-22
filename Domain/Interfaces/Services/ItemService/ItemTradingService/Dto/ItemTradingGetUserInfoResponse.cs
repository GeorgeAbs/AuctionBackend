namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto
{
    public class ItemTradingGetUserInfoResponse
    {
        public string UserName { get; private set; } = string.Empty;

        public string Title { get; private set; } = string.Empty;

        public float Rating { get; private set; }

        public string LogoURL { get; private set; } = string.Empty;

        public bool IsUserAsShop { get; private set; }

        public int ReviewsCount { get; private set; }

        public void SetInfo(string userName, string title, float rating, string logoURL, bool isUserAsShop, int reviewsCount)
        {
            UserName = userName;
            Title = title;
            Rating = rating;
            LogoURL = logoURL;
            IsUserAsShop = isUserAsShop;
            ReviewsCount = reviewsCount;
        }
    }
}
