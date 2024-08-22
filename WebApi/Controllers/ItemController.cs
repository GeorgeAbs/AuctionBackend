using Domain.Constants;
using Domain.Entities.Items.ItemTrading;
using Domain.Interfaces.Services.ItemService.ItemTradingService;
using Domain.Interfaces.Services.ItemService.ItemTradingService.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WebApi.DTO.Item.Request;

namespace WebApi.Controllers
{
    [Route("[controller]/[action]")]
    public class ItemController : ApiController
    {
        IItemTradingService<ItemTrading> _itemService;

        public ItemController(IItemTradingService<ItemTrading> itemService) 
        {
            _itemService = itemService;
        }

        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(null, "Get item by specified id. For everybody")]
        [SwaggerResponse(200, "Returns item info", typeof(ItemTradingGetResponse))]
        [SwaggerResponse(404, "Returns nothing")]
        [SwaggerResponse(409, "Returns message", typeof(string))]
        public async Task<IResult> Get([FromQuery] Guid id)
        {
            var res = await _itemService.GetAsync(id);

            if (res.Result == Domain.CoreEnums.Enums.MethodResults.NotFound) return Results.NotFound();

            if (res.Result == Domain.CoreEnums.Enums.MethodResults.Conflict) return Results.Conflict(res.Messages);

            return Results.Ok(res.ResultEntity);
        }

        [Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpPost]
        [SwaggerOperation(null, "Creates non auction item. User id will be retrieved from jwt. Only for default user")]
        [SwaggerResponse(200,
            "Returns success messages",
            typeof(List<string>))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IResult> CreateUpdateItemAsync([FromBody] CreateUpdateItemRequest dto)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Results.Unauthorized();
            }

            var result = await _itemService.CreateUpdateItemAsync(userId, dto.ItemInfo);

            if (result.Result == Domain.CoreEnums.Enums.MethodResults.Conflict) { return Results.Conflict(result.Messages);}

            return Results.Ok(result.Messages);
        }

        [Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpPost]
        [SwaggerOperation(null, "Creates auction item. User id will be retrieved from jwt. Only for default user")]
        [SwaggerResponse(200,
            "Returns success messages",
            typeof(List<string>))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IResult> CreateUpdateAuctionItemAsync([FromBody]CreateUpdateAuctionItemRequest dto)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Results.Unauthorized();
            }

            var result = await _itemService.CreateUpdateAuctionItemAsync(userId, dto.ItemInfo);

            if (result.Result == Domain.CoreEnums.Enums.MethodResults.Conflict) { return Results.Conflict(result.Messages); }

            return Results.Ok(result.Messages);
        }

        [Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpPost]
        [SwaggerOperation(null, "Returns item for changing. User id will be retrieved from jwt. Only for default user")]
        [SwaggerResponse(200,
            "Returns success messages",
            typeof(ItemTradingTemplateResponse))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IResult> GetTemplateAsync([FromQuery]Guid itemId)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Results.Unauthorized();
            }

            var result = await _itemService.GetItemTemplateAsync(userId, itemId);

            if (result.Result == Domain.CoreEnums.Enums.MethodResults.Conflict) { return Results.Conflict(result.Messages); }

            return Results.Ok(result.ResultEntity);
        }
    }
}
