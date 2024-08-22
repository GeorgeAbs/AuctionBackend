using Domain.Interfaces.Services.ItemService.ItemTradingService.Dto.AuctionItem;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApi.DTO.Item.Request
{
    public class CreateUpdateAuctionItemRequest
    {
        public ItemTradingAuctionInfoRequest ItemInfo { get;  set; }

        public CreateUpdateAuctionItemRequest()
        {
            
        }
    }
}
