using Domain.Constants;
using Domain.Entities.Addresses;
using Domain.Entities.Messages;
using Domain.Entities.PaymentMethods;
using Domain.Entities.Reviews;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.UserEntity
{
    public class User : IdentityUser<Guid>
    {
        public string? RefreshToken { get; private set; }

        public DateTime RefreshTokenExpiryTime { get; private set; }

        public string FirstName { get; private set; } = string.Empty;

        public string SecondName { get; private set; } = string.Empty;

        public string EmailConfirmationCode { get; private set; }

        public DateTime EmailConfirmationCodeSendingDate { get; set; } = DateTime.UtcNow;

        public string PhoneActivationCode { get; set; } = string.Empty;

        public DateTime PhoneConfirmationCodeSendingDate { get; set; } = DateTime.UtcNow;

        public float Rating { get; private set; } = RatingConstants.BASE_RATING;

        public DateTime RegistrationTime { get; set; } = DateTime.UtcNow;

        public DateTime BirthDate { get; private set; } = DateTime.UtcNow;

        public bool IsEmailActivated { get; set; } = false;

        public new string Email { get; private set; }

        public string Login { get; private set; } = string.Empty;

        public string TemporaryEmail { get; private set; } = string.Empty;

        public string VkLink { get; private set; } = string.Empty;

        public string FacebookLink { get; private set; } = string.Empty;

        public string TelegramLink { get; private set; } = string.Empty;

        public string WhatsappLink { get; private set; } = string.Empty;

        public List<UserAddress> Addresses { get; private set; } = [];

        public List<PaymentMethod> PaymentMethods { get; private set; } = [];

        public int DefaultDaysForShipment { get; private set; } = 7;

        /// <summary>
        /// user logo
        /// </summary>
        public string UserLogoBigImage { get; private set; } = "";
        public string UserLogoSmallImage { get; private set; } = "";


        /// <summary>
        /// switching option for user <-> userAsShop displaying info and some functions in future
        /// </summary>
        public bool IsUserAsShopOption { get; private set; } = false;

        #region user as shop props
        public string ShopTitle { get; private set; } = string.Empty;

        public string ShopDescription { get; private set; } = string.Empty;

        public string ShopLogoBigImage { get; private set; } = "";

        public string ShopLogoSmallImage { get; private set; } = "";
        #endregion

        public List<Review> Reviews { get; private set; } = [];

        public List<ItemTradingReview> ItemTradingReviewAboutMe { get; private set; } = [];

        private User() { }

        public User(
            string userName,
            DateTime registrationTime,
            string email,
            string emailActivationCode,
            string phoneActivationCoe)
        {
            UserName = userName;
            RegistrationTime = registrationTime;
            Email = email;
            EmailConfirmationCode = emailActivationCode;
            PhoneActivationCode = phoneActivationCoe;
        }

        public void SetRefreshToken(string refreshToken, DateTime refreshTokenExpiryTime)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
        }

        public void EraseRefreshToken()
        {
            RefreshToken = null;
        }

        public void ChangeUserLogo(string bigImage, string smallImage)
        {
            UserLogoBigImage = bigImage;
            UserLogoSmallImage = smallImage;
        }

        public void ChangeShopLogo(string bigImage, string smallImage)
        {
            ShopLogoBigImage = bigImage;
            ShopLogoSmallImage = smallImage;
        }


        public void ChangeUserInfo(string firstName,
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
            DefaultDaysForShipment = daysForShipment;
        }

        public void SetTempEmail(string tempEmail)
        {
            TemporaryEmail = tempEmail;
        }

        public void SetEmailConfirmationCode(string code)
        {
            EmailConfirmationCode = code;
        }

    }
}
