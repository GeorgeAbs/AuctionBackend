using Domain.CoreEnums;
using Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration.PropsNames.Request;

namespace Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration
{
    public class CatalogFilterRequest
    {
        public int PageNumber { get; }

        public int PageSize { get; }

        public string CategoryName { get; }

        public float PriceMin { get; }

        public float PriceMax { get; }

        public Enums.SortingMethods SortingMethod { get; }

        public IEnumerable<FilterFloatNameRequest> FilterFloatNames { get; }

        public IEnumerable<FilterIntNameRequest> FilterIntNames { get; }

        public IEnumerable<FilterStringNameRequest> FilterStringNames { get; }

        public IEnumerable<FilterBoolNameRequest> FilterBoolNames { get; }

        public Enums.SellingTypes SellingType { get; }

        public CatalogFilterRequest(int pageNumber,
            int pageSize,
            string categoryName,
            float priceMin,
            float priceMax,
            Enums.SortingMethods sortingMethod,
            IEnumerable<FilterFloatNameRequest> filterFloatNames,
            IEnumerable<FilterIntNameRequest> filterIntNames,
            IEnumerable<FilterStringNameRequest> filterStringNames,
            IEnumerable<FilterBoolNameRequest> filterBoolNames,
            Enums.SellingTypes sellingType)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            CategoryName = categoryName;
            PriceMin = priceMin;
            PriceMax = priceMax;
            SortingMethod = sortingMethod;
            FilterFloatNames = filterFloatNames;
            FilterIntNames = filterIntNames;
            FilterStringNames = filterStringNames;
            FilterBoolNames = filterBoolNames;
            SellingType = sellingType;
        }
    }
}
