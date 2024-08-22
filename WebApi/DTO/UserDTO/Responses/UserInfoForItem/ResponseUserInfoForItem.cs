namespace WebApi.Dto.UserDto.Responses.UserInfoForItem
{
    public class ResponseUserInfoForItem
    {
        public string? FirstName { get; set; }

        public string? SecondName { get; set; }

        public string? UserLogo { get; set; }

        public bool IsUserAsShopOption { get; set; }

        public string? ShopTitle { get; set; }

        public string? ShopDescription { get; set; }

        public string? ShopLogo { get; set; }

        public ResponseUserInfoForItem(string? firstName,
            string? secondName,
            string? userLogo,
            bool isUserAsShopOption,
            string? shopTitle,
            string? shopDescription,
            string? shopLogo)
        {
            FirstName = firstName;
            SecondName = secondName;
            UserLogo = userLogo;
            IsUserAsShopOption = isUserAsShopOption;
            ShopTitle = shopTitle;
            ShopDescription = shopDescription;
            ShopLogo = shopLogo;
        }
    }
}
