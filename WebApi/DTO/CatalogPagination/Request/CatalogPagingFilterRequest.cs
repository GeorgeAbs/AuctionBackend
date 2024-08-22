using Application;
using Microsoft.AspNetCore.Mvc;
using static Domain.CoreEnums.Enums;
using System.ComponentModel.DataAnnotations;
using Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration.PropsNames.Request;

namespace WebApi.DTO.CatalogPagination.Request
{
    public class CatalogPagingFilterRequest
    {
        [FromQuery(Name = "page")]
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [FromQuery(Name = "size")]
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = Settings.HISTORY_PAGE_SIZE;

        [FromQuery(Name = "sort")]
        public SortingMethods Sorting { get; set; } = SortingMethods.PriceAsc;

        [FromQuery(Name = "category")]
        public string CategoryName { get; set; } = "";

        [FromQuery(Name = "min-price")]
        public float MinPrice { get; set; } = 0f;

        [FromQuery(Name = "max-price")]
        public float MaxPrice { get; set; } = 99999999999f;

        [FromQuery(Name = "f")]
        public IEnumerable<FilterFloatNameRequest> FilterFloatNames { get; set; } = [];

        [FromQuery(Name = "i")]
        public IEnumerable<FilterIntNameRequest> FilterIntNames { get; set; } = [];

        [FromQuery(Name = "s")]
        public IEnumerable<FilterStringNameRequest> FilterStringNames { get; set; } = [];

        [FromQuery(Name = "b")]
        public IEnumerable<FilterBoolNameRequest> FilterBoolNames { get; set; } = [];

        [FromQuery(Name = "sell")]
        public SellingTypes SellingType { get; set; } = SellingTypes.All;

        public CatalogPagingFilterRequest()
        {
            
        }
    }
}
