using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto.AuctionItem
{
    public class ItemTradingAuctionSlotInfoRequest
    {
        [Required]
        [MinLength(1)]
        public string Title { get; set; }

        [Range(1, 6)]
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

        [Required]
        public string ImageBase64 { get; set; }

        public ItemTradingAuctionSlotInfoRequest()
        {

        }
    }
}
