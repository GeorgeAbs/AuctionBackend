using Domain.Interfaces.Services.MyProfileService.Dto.Address;
using Domain.Interfaces.Services.MyProfileService.Dto.Catalog;
using static Domain.CoreEnums.Enums;

namespace Domain.Interfaces.Services.MyProfileService.Dto
{
    public class MyProfileItemTradingCreationChangingFilter
    {
        public List<MyProfileCatalogCategory> RootCatalogCategories { get; set; }

        public List<MyProfileAddressDto> Adresses { get; set; }

        public List<PaymentMethod> PaymentMethods { get; set; }

        public MyProfileItemTradingCreationChangingFilter(List<MyProfileCatalogCategory> rootCatalogCategories, List<MyProfileAddressDto> adresses, List<PaymentMethod> paymentMethods)
        {
            RootCatalogCategories = rootCatalogCategories;
            Adresses = adresses;
            PaymentMethods = paymentMethods;
        }
    }
}
