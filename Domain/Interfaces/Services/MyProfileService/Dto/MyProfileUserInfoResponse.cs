namespace Domain.Interfaces.Services.MyProfileService.Dto
{
    public class MyProfileUserInfoResponse
    {
        public string UserName { get; private set; } = string.Empty;

        public string FirstName { get; private set; } = string.Empty;

        public string SecondName { get; private set; } = string.Empty;

        public DateTime BirthDate { get; private set; }

        public string Email { get; private set; } = string.Empty;

        public string PhoneNumber { get; private set; } = string.Empty;

        public string VkLink { get; private set; } = string.Empty;

        public string FacebookLink { get; private set; } = string.Empty;

        public string TelegramLink { get; private set; } = string.Empty;

        public string WhatsappLink { get; private set; } = string.Empty;

        public string UserLogoSmallImage { get; private set; } = string.Empty;

        public string UserLogoBigImage { get; private set; } = string.Empty;

        public bool IsUserAsShopOption { get; private set; } = false;

        public string ShopTitle { get; private set; } = string.Empty;

        public string ShopDescription { get; private set; } = string.Empty;

        public string ShopLogoSmallImage { get; private set; } = string.Empty;

        public string ShopLogoBigImage { get; private set; } = string.Empty;

        public int DefaultDaysForShipment { get; private set; }

        public List<string> Roles { get; private set; } = [];

        public void SetInfo(string userName,
            string firstName,
            string secondName,
            DateTime birthDate,
            string email,
            string phoneNumber,
            string vkLink,
            string facebookLink,
            string telegramLink,
            string whatsappLink,
            string userLogoSmallImage,
            string userLogoBigImage,
            bool isUserAsShopOption,
            string shopTitle,
            string shopDescription,
            string shopLogoSmallImage,
            string shopLogoBigImage,
            int defaultDaysForShipment,
            List<string> roles)
        {
            UserName = userName;
            FirstName = firstName;
            SecondName = secondName;
            BirthDate = birthDate;
            Email = email;
            PhoneNumber = phoneNumber;
            VkLink = vkLink;
            FacebookLink = facebookLink;
            TelegramLink = telegramLink;
            WhatsappLink = whatsappLink;
            UserLogoSmallImage = userLogoSmallImage;
            UserLogoBigImage = userLogoBigImage;
            IsUserAsShopOption = isUserAsShopOption;
            ShopTitle = shopTitle;
            ShopDescription = shopDescription;
            ShopLogoSmallImage = shopLogoSmallImage;
            ShopLogoBigImage = shopLogoBigImage;
            DefaultDaysForShipment = defaultDaysForShipment;
            Roles = roles;
        }
    }
}
