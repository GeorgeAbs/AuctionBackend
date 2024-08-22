using Domain.CoreEnums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto
{
    public class ItemTradingInfoRequest
    {
        public Guid ItemId { get; set; } = Guid.NewGuid();

        [Required]
        public string CatalogSystemName { get;  set; }

        [Required]
        [MinLength(4)]
        public string Title { get; set; }

        public string Description { get; set; } = string.Empty ;

        [Required]
        [Range(1, 9999999)]
        public int Price { get; set; }

        public IEnumerable<IntPropertyRequest> IntProperties { get; set; }

        public IEnumerable<FloatPropertyRequest> FloatProperties { get; set; }

        public IEnumerable<StringPropertyRequest> StringProperties { get; set; }

        public IEnumerable<BoolPropertyRequest> BoolProperties { get; set; }

        [MinLength(1)]
        public IReadOnlyCollection<AddressItemCreationRequest> Addresses { get; set; } = [];

        public IEnumerable<Enums.PaymentMethod> PaymentMethods { get; set; }

        [Range(1,9999999)]
        public int Quantity { get; set; } = 1;

        [MinLength(1)]
        [MaxLength(4)]
        public IEnumerable<string> ImagesBase64 { get; set; } = [];

        public ItemTradingInfoRequest()
        {

        }
    }
}
