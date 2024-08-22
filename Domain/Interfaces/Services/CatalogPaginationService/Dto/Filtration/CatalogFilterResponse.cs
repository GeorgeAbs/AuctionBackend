using Domain.Common.Filters;
using Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration.PropsNames.Responce;
using System.Text.Json.Serialization;

namespace Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration
{
    public class CatalogFilterResponse
    {
        public string CategoryDisplayName { get; } = string.Empty;

        public CatalogPriceFilterResponse? PriceFilter { get; }

        public IEnumerable<FilterBoolPropertyNameResponse>? FilterBoolPropertyNames { get; }

        public IEnumerable<FilterStringPropertyNameResponse>? FilterStringPropertyNames { get; }

        public IEnumerable<FilterFloatPropertyNameResponse>? FilterFloatPropertyNames { get; }

        public IEnumerable<FilterIntPropertyNameResponse>? FilterIntPropertyNames { get; }

        public IReadOnlyCollection<SortingItem> SortingPriceDateFilter { get; } = Filters.SortingPriceDateFilter;

        public IReadOnlyCollection<SellingTypeFilterItem> SellingTypeFilter { get; } = Filters.SellingTypeFilter;

        [JsonConstructor]
        public CatalogFilterResponse(string categoryDisplayName,
            CatalogPriceFilterResponse? priceFilter = null,
            IEnumerable<FilterBoolPropertyNameResponse>? filterBoolPropertyNames = null,
            IEnumerable<FilterStringPropertyNameResponse>? filterStringPropertyNames = null,
            IEnumerable<FilterFloatPropertyNameResponse>? filterFloatPropertyNames = null,
            IEnumerable<FilterIntPropertyNameResponse>? filterIntPropertyNames = null )
        {
            CategoryDisplayName = categoryDisplayName;
            PriceFilter = priceFilter;
            FilterBoolPropertyNames = filterBoolPropertyNames;
            FilterStringPropertyNames = filterStringPropertyNames;
            FilterFloatPropertyNames = filterFloatPropertyNames;
            FilterIntPropertyNames = filterIntPropertyNames;
        }
    }
}
