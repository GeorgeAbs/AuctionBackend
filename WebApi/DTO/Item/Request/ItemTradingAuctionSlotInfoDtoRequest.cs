using Domain.Interfaces.Services.ItemService.ItemTradingService.Dto;
using System.ComponentModel.DataAnnotations;

namespace WebApi.DTO.Item.Request
{
    public class ItemTradingAuctionSlotInfoDtoRequest
    {
        [Range(1, 1000)]
        public int SlotNumber { get; set; } = 1;

        public string Description { get; set; } = "";

        [Range(1, 100000000)]
        public int Price { get; set; }

        [Range(1, 100000000)]
        public int MinimumBid { get; set; }

        [Range(1, 100000000)]
        public int BlitzPrice { get; set; }

        public IEnumerable<IntPropertyRequest> IntProperties { get; set; } = [];

        public IEnumerable<FloatPropertyRequest> FloatProperties { get; set; } = [];

        public IEnumerable<StringPropertyRequest> StringProperties { get; set; } = [];

        public IEnumerable<BoolPropertyRequest> BoolProperties { get; set; } = [];

        public ItemTradingAuctionSlotInfoDtoRequest()
        {

        }
    }
}
