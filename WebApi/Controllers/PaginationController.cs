using Domain.Interfaces.Services.CatalogPaginationService;
using Domain.Interfaces.Services.CatalogPaginationService.Dto;
using Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using WebApi.DTO.CatalogPagination.Request;
using WebApi.ModelBinders;
using static Domain.CoreEnums.Enums;

namespace WebApi.Controllers
{
    public class PaginationController : ApiController
    {
        private readonly ICatalogPaginationService _paginationService;

        public PaginationController(ICatalogPaginationService paginationService)
        {
            _paginationService = paginationService;
        }

        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(null,
            "Should use then user filters category button." +
            " Request example: /category111/filter?page=1&size=20&sort=2&min-price=1&max-price=1000&sell=3&f=propName-minValue-maxValue&i=propName-minValue-maxValue&s=propName-propValue~propName-propValue~propName-propValue&b=propName-propValue~propName-propValue~propName-propValue." +
            " Returns pagination view model: items, filter, paging info ")]
        [SwaggerResponse(200,
            "Returns pagination view model",
            typeof(CatalogPaginationResponse))]
        [SwaggerResponse(409,
            "Returns list of messages",
            typeof(List<string>))]
        [Route("catalog/{category}/filter")]
        public async Task<IResult> FilteringAsync([ModelBinder(BinderType = typeof(CatalogPaginationModelBinder))] CatalogPagingFilterRequest filter)
        {
            var newFilter = new CatalogFilterRequest(filter.PageNumber,
                filter.PageSize,
                filter.CategoryName,
                filter.MinPrice,
                filter.MaxPrice,
                filter.Sorting,
                filter.FilterFloatNames,
                filter.FilterIntNames,
                filter.FilterStringNames,
                filter.FilterBoolNames,
                filter.SellingType);

            var res = await _paginationService.GetPaginationViewModelByFilterAsync(newFilter);

            if (res.Result == MethodResults.Conflict) return Results.Conflict(res.Messages);

            return Results.Ok(res.ResultEntity);
        }

        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(null,
            "Should use then user clicks on catalog category button (inc. page, price and sorting filter). Returns pagination view model: items, filter, paging info ")]
        [SwaggerResponse(200,
            "Returns pagination view model",
            typeof(CatalogPaginationResponse))]
        [SwaggerResponse(409,
            "Returns list of messages",
            typeof(List<string>))]
        [Route("catalog/{category}")]
        public async Task<IResult> FullCategoryFilteringAsync(
            string category,
            [FromQuery(Name = "page")][Range(1, int.MaxValue)] int page = 1,
            [FromQuery(Name = "size")][Range(1, int.MaxValue)] int size = 20,
            [FromQuery(Name = "min-price")] float minPrice = 1,
            [FromQuery(Name = "max-price")] float maxPrice = 9999999,
            [FromQuery(Name = "sort")] SortingMethods sorting = SortingMethods.DateDesc,
            [FromQuery(Name = "sell")] SellingTypes sellingType = SellingTypes.All)
        {
            var newFilter = new CatalogFilterRequest(page,
                size,
                category,
                minPrice,
                maxPrice,
                sorting,
                [],
                [],
                [],
                [],
                sellingType);

            var res = await _paginationService.GetCategoryFullFilteringAsync(newFilter);

            if (res.Result == MethodResults.Conflict) return Results.Conflict(res.Messages);

            return Results.Ok(res.ResultEntity);
        }

    }
}
