using Domain.BackendResponses;
using Domain.Entities.Items;
using Domain.Interfaces;
using Domain.Interfaces.DbContexts;
using Domain.Interfaces.Services;

namespace WebApi
{
    public class UserNotifier : IUserNotifier
    {
        private readonly IUserDbContext _userDbContext;
        private readonly ICatalogDbContext _catalogDbContext;

        public UserNotifier(IUserDbContext userDbContext, ICatalogDbContext catalogDbContext)
        {
            _userDbContext = userDbContext;
            _catalogDbContext = catalogDbContext;
        }

        public async Task<MethodResult> NotifyAuctionIsEndedForOwnerAsync(Guid userId, ItemBase item)
        {
            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        public async Task<MethodResult> NotifyAuctionIsEndedWithoutWinnerForOwnerAsync<TBid>(Guid userId, IAuctionSlot<TBid> slot) where TBid : class
        {
            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        public async Task<MethodResult> NotifyAuctionIsEndedWithWinnerForOwnerAsync<TBid>(Guid userId, IAuctionSlot<TBid> slot) where TBid : class
        {
            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        public async Task<MethodResult> NotifyAuctionIsEndedWithoutVictoryForBidderAsync<TBid>(Guid userId, IAuctionSlot<TBid> slot) where TBid : class
        {
            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        public async Task<MethodResult> NotifyAuctionIsEndedWithVictoryForBidderAsync<TBid>(Guid userId, IAuctionSlot<TBid> slot) where TBid : class
        {
            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        public async Task<MethodResult> NotifyBiggestBidOwnerChangedForBidderAsync<TBid>(Guid userId, IAuctionSlot<TBid> slot) where TBid : class
        {
            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }
    }
}
