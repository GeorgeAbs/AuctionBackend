using Domain.Entities.UserEntity;

namespace WebApi.Dto.UserDto.Responses.UserInfoForMyProfile
{
    public static class ResponseUserInfoForMyProfileMapper
    {
        public static ResponseUserInfoForMyProfile MapToDto(User user)
        {
            return new ResponseUserInfoForMyProfile(
                firstName: user.FirstName,
                secondName: user.SecondName,
                birthDate: user.BirthDate,
                email: user.Email,
                phoneNumber: user.PhoneNumber,
                vkLink: user.VkLink,
                facebookLink: user.FacebookLink,
                telegramLink: user.TelegramLink,
                whatsappLink: user.WhatsappLink,
                userLogo: user.UserLogoSmallImage,
                isUserAsShopOption: user.IsUserAsShopOption,
                shopTitle: user.ShopTitle,
                shopDescription: user.ShopDescription,
                shopLogo: user.ShopLogoSmallImage);
        }
    }
}
