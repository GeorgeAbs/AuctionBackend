using Domain.Interfaces.Services.ItemService.ItemTradingService.Dto.AuctionItem;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApi.DTO.Item.Request
{
    public class CreateUpdateAuctionItemDtoRequest
    {
        public Guid ItemId { get; set; } = Guid.Empty;

        [Required]
        public string CatalogName { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime AuctionEndingTime { get; set; } = DateTime.UtcNow;

        [Required]
        public int DaysForShipment { get; set; } = 1;

        [Required]
        public IEnumerable<Guid> PaymentMethods { get; set; }

        [Required]
        public IReadOnlyCollection<Guid> Addresses { get; set; }

        [MinLength(1)]
        public IEnumerable<ItemTradingAuctionSlotInfoDtoRequest> Slots { get; set; } = [];

        public CreateUpdateAuctionItemDtoRequest() { }
    }
}
