using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services.MyProfileService.Dto
{
    public class MyProfileUserShortInfoResponse
    {
        public string UserName { get; private set; } = string.Empty;

        public string UserLogoSmallImage { get; private set; } = string.Empty;

        public string UserLogoBigImage { get; private set; } = string.Empty;

        public bool IsUserAsShopOption { get; private set; } = false;

        public string ShopTitle { get; private set; } = string.Empty;

        public string ShopDescription { get; private set; } = string.Empty;

        public string ShopLogoSmallImage { get; private set; } = string.Empty;

        public string ShopLogoBigImage { get; private set; } = string.Empty;

        public void SetInfo(string userName,
            string userLogoSmallImage,
            string userLogoBigImage,
            bool isUserAsShopOption,
            string shopTitle,
            string shopDescription,
            string shopLogoSmallImage,
            string shopLogoBigImage)
        {
            UserName = userName;
            UserLogoSmallImage = userLogoSmallImage;
            UserLogoBigImage = userLogoBigImage;
            IsUserAsShopOption = isUserAsShopOption;
            ShopTitle = shopTitle;
            ShopDescription = shopDescription;
            ShopLogoSmallImage = shopLogoSmallImage;
            ShopLogoBigImage = shopLogoBigImage;
        }
    }
}
