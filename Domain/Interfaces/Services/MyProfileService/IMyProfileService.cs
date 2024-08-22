using Domain.BackendResponses;
using Domain.Common.Dto;
using Domain.Interfaces.Services.MyProfileService.Dto;
using Domain.Interfaces.Services.MyProfileService.Dto.Address;
using Domain.Interfaces.Services.MyProfileService.Dto.Comment;
using Domain.Interfaces.Services.MyProfileService.Dto.History;
using Domain.Interfaces.Services.MyProfileService.Dto.Review;
using static Domain.CoreEnums.Enums;

namespace Domain.Interfaces.Services.MyProfileService
{
    public interface IMyProfileService
    {
        public Task<MethodResult<MyProfileUserInfoResponse>> GetUserInfoAsync(Guid userId);

        public Task<MethodResult<MyProfileUserShortInfoResponse>> GetUserShortInfoAsync(Guid userId);

        public Task<MethodResult<ImageDto>> ChangeUserLogoAsync(Guid userId, Stream fileStream);

        public Task<MethodResult<ImageDto>> ChangeShopLogoAsync(Guid userId, Stream fileStream);

        public Task<MethodResult> ChangeUserNameAsync(Guid userId, string newUserName);

        public Task<MethodResult> ChangePwdAsync(Guid userId, string oldPwd, string newPwd);

        public Task<MethodResult> ChangeUserInfoAsync(Guid userId,
            string firstName,
            string secondName,
            DateTime birthDate,
            string vkLink,
            string facebookLink,
            string telegramLink,
            string whatsappLink,
            bool isUserAsShopOption,
            string shopTitle,
            string shopDescription,
            int daysForShipment);

        public Task<MethodResult> ChangeEmailAsync(Guid userId, string newEmail);

        public Task<MethodResult> ConfirmChangingEmailAsync(Guid userId, string confirmationCode);

        public Task<MethodResult> ChangePhoneAsync(Guid userId, string newEmail);

        public Task<MethodResult> ConfirmChangingPhoneAsync(Guid userId, string confirmationCode);

        public Task<MethodResult<PaginationViewModel<MyProfileItemTradingHistoryEntity>>> GetPurchaseHistoryAsync(Guid userId,
            int pageNumber,
            int pageSize,
            ItemTradingStatus itemStatus,
            SortingMethods sorting,
            DateTime dateFrom,
            DateTime dateTo);

        public Task<MethodResult<PaginationViewModel<MyProfileItemTradingHistoryEntity>>> GetMyItemsHistoryAsync(Guid userId,
           int pageNumber,
           int pageSize,
           ItemTradingStatus itemStatus,
           SortingMethods sorting,
           DateTime dateFrom,
           DateTime dateTo);

        public Task<MethodResult<PaginationViewModel<MyProfileItemTradingHistoryEntity>>> GetAuctionHistoryAsync(Guid userId,
           int pageNumber,
           int pageSize,
           ItemTradingStatus itemStatus,
           SortingMethods sorting,
           DateTime dateFrom,
           DateTime dateTo);

        public Task<MethodResult<PaginationViewModel<MyProfileReviewResponseEntity>>> GetProductsReviewsAsync(Guid currentUserId,
           int pageNumber,
           int pageSize,
           ReviewCommentsTypes reviewCommentType,
           SortingMethods sorting,
           DateTime dateFrom,
           DateTime dateTo);

        public Task<MethodResult<PaginationViewModel<MyProfileCommentResponseEntity>>> GetProductsCommentsAsync(Guid currentUserId,
           int pageNumber,
           int pageSize,
           ReviewCommentsTypes reviewCommentType,
           SortingMethods sorting,
           DateTime dateFrom,
           DateTime dateTo);

        public Task<MethodResult<IEnumerable<MyProfileAddressDto>>> GetUserAddressesAsync(Guid userId);

        public Task<MethodResult> CreateChangeAddressAsync(Guid userId, MyProfileAddressDto adressDto);

        public Task<MethodResult> DeleteAddressAsync(Guid userId, Guid adressId);

        public Task<MethodResult<MyProfileItemTradingCreationChangingFilter>> GetFilterForItemCreationChangingAsync(Guid userId);
    }
}
