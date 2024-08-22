using Domain.BackendResponses;

namespace Domain.Interfaces.Services.ItemService
{
    public interface IItemTradingStatusesService
    {
        public Task<MethodResult> FromTemplateToModerationAsync(Guid userId, Guid itemId);

        public Task<MethodResult> FromPublishedToTemplateAsync(Guid userId, Guid itemId);

        public Task<MethodResult> FromModerationToTemplateAsync(Guid userId, Guid itemId);

        public Task<MethodResult> FromModerationToPublishedAsync(Guid userId, Guid itemId);

        public Task<MethodResult> FromDeclinedByModeratorToTemplateAsync(Guid userId, Guid itemId);

        public Task<MethodResult> FromPublishedToLockedByAuctionAsync(Guid itemId);

        public Task<MethodResult> FromPublishedToAuctionIsEndedAsync(Guid itemId);

        public Task<MethodResult> FromLockedByAuctionToAuctionIsEndedAsync(Guid itemId);

        public Task<MethodResult> FromAuctionIsEndedToTemplateAsync(Guid userId, Guid itemId);
    }
}
