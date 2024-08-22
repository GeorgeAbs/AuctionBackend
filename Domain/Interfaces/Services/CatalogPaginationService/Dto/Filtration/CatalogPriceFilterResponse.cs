using System.Text.Json.Serialization;

namespace Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration
{
    public class CatalogPriceFilterResponse
    {
        public float MinPrice {  get; }

        public float MaxPrice { get; }

        [JsonConstructor]
        public CatalogPriceFilterResponse(float minPrice, float maxPrice)
        {
            MinPrice = minPrice;
            MaxPrice = maxPrice;
        }
    }
}
