using Domain.BackendResponses;
using Domain.Entities.Items;

namespace Domain.Interfaces.Services
{
    public interface IUserNotifier
    {
        public Task<MethodResult> NotifyAuctionIsEndedForOwnerAsync(Guid userId, ItemBase item);

        public Task<MethodResult> NotifyAuctionIsEndedWithWinnerForOwnerAsync<TBid>(Guid userId, IAuctionSlot<TBid> slot) where TBid : class;

        public Task<MethodResult> NotifyAuctionIsEndedWithoutVictoryForBidderAsync<TBid>(Guid userId, IAuctionSlot<TBid> slot) where TBid : class;

        public Task<MethodResult> NotifyAuctionIsEndedWithVictoryForBidderAsync<TBid>(Guid userId, IAuctionSlot<TBid> slot) where TBid : class;

        public Task<MethodResult> NotifyBiggestBidOwnerChangedForBidderAsync<TBid>(Guid userId, IAuctionSlot<TBid> slot) where TBid : class;
    }
}
