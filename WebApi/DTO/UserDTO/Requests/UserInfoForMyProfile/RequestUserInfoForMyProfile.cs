using System.ComponentModel.DataAnnotations;

namespace WebApi.Dto.UserDto.Requests.UserInfoForMyProfile
{
    public class RequestUserInfoForMyProfile
    {
        public string FirstName { get; private set; }

        public string SecondName { get; private set; }

        public DateTime BirthDate { get; private set; }

        public string VkLink { get; private set; }

        public string FacebookLink { get; private set; }

        public string TelegramLink { get; private set; }

        public string WhatsappLink { get; private set; }

        public bool IsUserAsShopOption { get; private set; }

        public string ShopTitle { get; private set; }

        public string ShopDescription { get; private set; }

        [Range(1, int.MaxValue)]
        public int DaysForShipment { get; private set; }

        public RequestUserInfoForMyProfile(string firstName,
            string secondName,
            DateTime birthDate,
            string vkLink,
            string facebookLink,
            string telegramLink,
            string whatsappLink,
            bool isUserAsShopOption,
            string shopTitle,
            string shopDescription,
            int daysForShipment)
        {
            FirstName = firstName;
            SecondName = secondName;
            BirthDate = birthDate;
            VkLink = vkLink;
            FacebookLink = facebookLink;
            TelegramLink = telegramLink;
            WhatsappLink = whatsappLink;
            IsUserAsShopOption = isUserAsShopOption;
            ShopTitle = shopTitle;
            ShopDescription = shopDescription;
            DaysForShipment = daysForShipment;
        }
    }
}
