using Domain.Interfaces.Services.ItemService.ItemTradingService.Dto;
using System.ComponentModel.DataAnnotations;

namespace WebApi.DTO.Item.Request
{
    public class CreateUpdateItemRequest
    {
        public ItemTradingInfoRequest ItemInfo { get; set; }

        public CreateUpdateItemRequest()
        {

        }
    }
}
