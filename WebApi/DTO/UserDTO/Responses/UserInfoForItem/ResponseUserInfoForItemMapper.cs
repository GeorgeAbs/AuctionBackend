using Domain.Entities.UserEntity;

namespace WebApi.Dto.UserDto.Responses.UserInfoForItem
{
    public static class ResponseUserInfoForItemMapper
    {
        public static ResponseUserInfoForItem MapToDto(User user)
        {
            if (user.IsUserAsShopOption)
                //if sholld show only shop info
                return new ResponseUserInfoForItem(
                firstName: null,
                secondName: null,
                userLogo: null,
                isUserAsShopOption: user.IsUserAsShopOption,
                shopTitle: user.ShopTitle,
                shopDescription: user.ShopDescription,
                shopLogo: user.ShopLogoSmallImage);

            //if sholld show only user info
            return new ResponseUserInfoForItem(
                firstName: user.FirstName,
                secondName: user.SecondName,
                userLogo: user.UserLogoSmallImage,
                isUserAsShopOption: user.IsUserAsShopOption,
                shopTitle: null,
                shopDescription: null,
                shopLogo: null);
        }
    }
}
