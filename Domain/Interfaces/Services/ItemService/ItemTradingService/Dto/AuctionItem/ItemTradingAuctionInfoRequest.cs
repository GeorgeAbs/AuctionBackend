using Domain.CoreEnums;
using System.ComponentModel.DataAnnotations;
namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto.AuctionItem
{
    public class ItemTradingAuctionInfoRequest
    {
        public Guid ItemId { get; set; } = Guid.Empty;

        [Required]
        public string CatalogSystemName { get; set; }

        [Required]
        [MinLength(4)]
        public string Title { get;  set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime AuctionEndingTime { get;  set; } = DateTime.UtcNow;

        [Required]
        [Range(1,999)]
        public int DaysForShipment { get; set; } = 1;

        [Required]
        public IEnumerable<Enums.PaymentMethod> PaymentMethods { get; set; }

        [MinLength(1)]
        public IReadOnlyCollection<AddressItemCreationRequest> Addresses { get; set; } = [];

        [MinLength(1)]
        [MaxLength(6)]
        public IEnumerable<ItemTradingAuctionSlotInfoRequest> Slots { get; set; } = [];

        [MinLength(1)]
        [MaxLength(4)]
        public IEnumerable<string> ImagesBase64 { get; set; } = [];

        public ItemTradingAuctionInfoRequest()
        {

        }
    }
}
