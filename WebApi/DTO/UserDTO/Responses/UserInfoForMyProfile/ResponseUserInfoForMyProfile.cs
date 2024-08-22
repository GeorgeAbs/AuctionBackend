namespace WebApi.Dto.UserDto.Responses.UserInfoForMyProfile
{
    public class ResponseUserInfoForMyProfile
    {
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string VkLink { get; set; }

        public string FacebookLink { get; set; }

        public string TelegramLink { get; set; }

        public string WhatsappLink { get; set; }

        public string UserLogo { get; set; }

        public bool IsUserAsShopOption { get; set; }

        public string ShopTitle { get; set; }

        public string ShopDescription { get; set; }

        public string ShopLogo { get; set; }

        public ResponseUserInfoForMyProfile(string firstName,
            string secondName,
            DateTime birthDate,
            string email,
            string phoneNumber,
            string vkLink,
            string facebookLink,
            string telegramLink,
            string whatsappLink,
            string userLogo,
            bool isUserAsShopOption,
            string shopTitle,
            string shopDescription,
            string shopLogo)
        {
            FirstName = firstName;
            SecondName = secondName;
            BirthDate = birthDate;
            Email = email;
            PhoneNumber = phoneNumber;
            VkLink = vkLink;
            FacebookLink = facebookLink;
            TelegramLink = telegramLink;
            WhatsappLink = whatsappLink;
            UserLogo = userLogo;
            IsUserAsShopOption = isUserAsShopOption;
            ShopTitle = shopTitle;
            ShopDescription = shopDescription;
            ShopLogo = shopLogo;
        }
    }
}
