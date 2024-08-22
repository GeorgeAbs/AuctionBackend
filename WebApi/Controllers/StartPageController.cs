using Domain.Interfaces.Services.AdminGlobalSettingsService;
using Domain.Interfaces.Services.AdminGlobalSettingsService.DTO;
using Domain.Interfaces.Services.CatalogPaginationService;
using Domain.Interfaces.Services.CatalogPaginationService.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [Route("[controller]/[action]")]
    public class StartPageController : ApiController
    {
        public readonly IAdminGlobalSettingsService _adminGlobalSettingsService;
        public readonly ICatalogPaginationService _catalogPaginationService;



        public StartPageController(IAdminGlobalSettingsService adminGlobalSettingsService, ICatalogPaginationService catalogPaginationService) 
        {
            _adminGlobalSettingsService = adminGlobalSettingsService;
            _catalogPaginationService = catalogPaginationService;
        }

        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(null,
            "Returns main page design info (banners, etc)")]
        [SwaggerResponse(200,
            "Returns design info",
            typeof(StartPageDesignInfo))]
        public async Task<IResult> GetDesignInfoAsync()
        {
            var res = await _adminGlobalSettingsService.GetStartPageDesignInfoAsync();

            return Results.Ok(res.ResultEntity);
        }

        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(null,
            "Returns list of popular items")]
        [SwaggerResponse(200,
            "Returns list of popular items",
            typeof(IEnumerable<ItemTradingPaginationResponse>))]
        public async Task<IResult> GetPopularItemsAsync()
        {
            var res = await _catalogPaginationService.GetPopularItemsAsync();

            return Results.Ok(res);
        }

        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(null,
            "Returns list of new items")]
        [SwaggerResponse(200,
            "Returns list of newly created items",
            typeof(IEnumerable<ItemTradingPaginationResponse>))]
        public async Task<IResult> GetNewItemsAsync()
        {
            var res = await _catalogPaginationService.GetNewItemsAsync();

            return Results.Ok(res);
        }
    }
}
