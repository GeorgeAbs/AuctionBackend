using Domain.Constants;
using Domain.Interfaces.Services.AuctionService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;
using System.Security.Claims;
using WebApi.Constants;

namespace WebApi.Hubs
{
    [SignalRHub(HubsPaths.BID_HUB_PATH,SignalRSwaggerGen.Enums.AutoDiscover.Inherit,null,null,null,false,false,"Hub for auction")]
    [Authorize(AuthenticationSchemes = "Identity.Application," + JwtBearerDefaults.AuthenticationScheme,
        Policy = null,
        Roles = $"{RoleConstants.DEFAULT_USER},{RoleConstants.SUPER_USER}")]
    public class BidHub : Hub
    {
        private readonly IBidsService _auctionService;

        public BidHub(IBidsService auctionService) 
        {
            _auctionService = auctionService;
        }

        [SignalRMethod("Initialize", SignalRSwaggerGen.Enums.Operation.Inherit, SignalRSwaggerGen.Enums.AutoDiscover.Inherit, null,
            "Add current connection to listening for slots groups as success (slot group = separated slot).\n" +
            "If slot with specified id is not found sends [{},{}...] list of messages to AuctionConflict method.\n\r" +
            "If auction is ended sends [{},{}...] list of messages to AuctionConflict method. Only for default user")]
        public async Task Initialize(IEnumerable<Guid> itemsIds)
        {
            foreach(var itemId in itemsIds)
            {
                var slot = await _auctionService.GetSlotForBidHubInitializingAsync(itemId);

                if (slot == null)
                {
                    await Clients.Caller.SendAsync("AuctionConflict", new List<string>() { ResponsesTextConstants.GENERAL_ERROR_MESSAGE }, itemId);
                    return;
                }

                if (slot.Status != Domain.CoreEnums.Enums.AuctionSlotStatus.Started || slot.AuctionEndingTime <= DateTime.UtcNow)
                {
                    await Clients.Caller.SendAsync("AuctionIsEnded", new List<string>() { "Аукцион закончен или не начался" }, itemId);
                    return;
                }

                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, itemId.ToString());
            }
        }

        public async Task PlaceBid(HubBid hubBid )
        {
            
            if (hubBid.slotId == Guid.Empty)
            {
                await Clients.Caller.SendAsync("AuctionConflict", new List<string>() { ResponsesTextConstants.GENERAL_ERROR_MESSAGE }, hubBid.slotId);
                return;
            }

            if(!Guid.TryParse(Context.GetHttpContext()?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId) || userId == Guid.Empty)
            {
                await Clients.Caller.SendAsync("AuctionConflict", new List<string>() { "Ошибка авторизации" }, hubBid.slotId);
                return;
            }

            var slot = await _auctionService.GetSlotForBiddingAsync(hubBid.slotId);

            if (slot == null)
            {
                await Clients.Caller.SendAsync("AuctionConflict", new List<string>() { ResponsesTextConstants.GENERAL_ERROR_MESSAGE }, hubBid.slotId);
                return;
            }

            if (slot.UserId == userId)
            {
                await Clients.Caller.SendAsync("AuctionConflict", new List<string>() { "Нельзя делать ставки являясь хозяином лота" }, hubBid.slotId);
                return;
            }

            if (slot.Status != Domain.CoreEnums.Enums.AuctionSlotStatus.Started || slot.AuctionEndingTime <= DateTime.UtcNow)
            {
                await Clients.Caller.SendAsync("AuctionIsEnded", new List<string>() { "Аукцион закончен" }, hubBid.slotId);
                return;
            }

            if (slot.MinimumBid > hubBid.bidAmount - slot.Price)
            {
                await Clients.Caller.SendAsync("AuctionConflict", new List<string>() { ResponsesTextConstants.BID_AMOUNT_ERROR }, hubBid.slotId);
                return;
            }

            var biddingRes = await _auctionService.ProceedBidAsync(userId, hubBid.bidAmount, slot);

            if (biddingRes.Result == Domain.CoreEnums.Enums.MethodResults.Conflict) 
            {
                await Clients.Caller.SendAsync("AuctionConflict", biddingRes.Messages, hubBid.slotId);
                return;
            }

            await Clients.Caller.SendAsync("AuctionSuccess", new List<string>() { ResponsesTextConstants.BID_IS_PLACED_SUCCESSFULLY}, hubBid.slotId);

            await Clients.Group(hubBid.slotId.ToString()).SendAsync("AuctionCurrentPrice", slot.Price, hubBid.slotId);

            //if auction is won,just send message. Winning procedure is in separate service
            if (slot.Price >= slot.BlitzPrice) 
            {
                await Clients.Group(hubBid.slotId.ToString()).SendAsync("AuctionIsEnded", new List<string>() { "Слот выкуплен" }, hubBid.slotId);
            }
        }
    }

    public class HubBid()
    {
        public int bidAmount {  get; set; }

        public Guid slotId { get; set; }
    }
}
